using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class SpatialInterface : MonoBehaviour
    {

        public float LapSummaryTimeIn;
        public float LapSummaryHoldTime;
        public float LapSummaryTimeOut;


        public Image InfomaticLine;
        public Text LapTime;
        public Text LapCount;
        public GameObject Rocket;
        private RocketRaceMetrics _metrics;
        private Transform _rocketTransform;
        // Use this for initialization
        private void Start()
        {
            _metrics = Rocket.GetComponent<RocketRaceMetrics>();
            _rocketTransform = Rocket.transform;
            _metrics.AddLapCompleteCallback(LapComplete);
        }

        private float _deltaOpacityIn;
        private float _deltaOpacityOut;
        private void LapComplete(GameObject rocket, TimeSpan lapTime, int lapNumber)
        {
            Debug.Log("Lap Completed");
            _lapTime = lapTime;
            _opacity = 0;
            LapCount.text = "Lap " + (lapNumber+1);
            if (LapSummaryTimeIn > 0)
            {
                _deltaOpacityIn = Time.fixedDeltaTime/LapSummaryTimeIn;
            }
            else
            {
                _deltaOpacityIn = 1;
            }
            if (LapSummaryTimeOut > 0)
            {
                _deltaOpacityOut = (Time.fixedDeltaTime / LapSummaryTimeOut);
            }
            else
            {
                _deltaOpacityOut= 1;
            }
            InvokeRepeating("LapEndIn",0,Time.fixedDeltaTime);       
        }

        private float _opacity= 0;
        private TimeSpan _lapTime;
        private TimeSpan _lapLabelTime;//The label is what is display in the interface
        private void LapEndIn()
        {
        
            _opacity += _deltaOpacityIn;
        
            _lapLabelTime = new TimeSpan((long) Mathf.Lerp(0, _lapTime.Ticks,_opacity));
            if (_opacity >=1f)
            {
                _opacity = 1f;
                _lapLabelTime = _lapTime;
                CancelInvoke("LapEndIn"); 
                InvokeRepeating("LapEndOut", LapSummaryHoldTime, Time.fixedDeltaTime);
            }
        }

        private void LapEndOut()
        {        
            _opacity -= _deltaOpacityOut;
            if (_opacity <=0f)
            {
                _opacity = 0;
                CancelInvoke("LapEndOut");
            }
        }



// Update is called once per frame
        void Update () {
        
            if (_opacity >=0)
            {                

                transform.position = _rocketTransform.position;
                LapTime.text = StaticUtils.ToRaceTimeString(_lapLabelTime);
                var opColor=new Color(1,1,1,_opacity);
                LapTime.color = opColor;
                InfomaticLine.color = opColor;
                LapCount.color = opColor;
                if (_opacity == 0f)
                {
                    _opacity = -1;
                }
            }
        }
    }
}

