using System;
using UnityEngine;
using UnityEngine.UI;
using RealRocketRacing.Rocket;
using RealRocketRacing.RaceCheckpoints;
namespace RealRocketRacing.Hud
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
		private TimeSpan _lastTime;
        // Use this for initialization
        private void Start()
        {
			if (Rocket != null) {

				_UIColor=Rocket.GetComponent<SpriteRenderer>().color;
				_metrics = Rocket.GetComponent<RocketRaceMetrics> ();
				_lastTime=_metrics.CurrentLapTime;
				_rocketTransform = Rocket.transform;
				_metrics.AddPassedCheckpointCallback (PassedCheckpoint);

			} else {
				gameObject.SetActive(false);
			}
        }

        private float _deltaOpacityIn;
        private float _deltaOpacityOut;
		private void PassedCheckpoint(Checkpoint checkpoint, GameObject rocket)
        {   

			if (_metrics.CurrentLapTime < _lastTime) {
				_lapTime = _metrics.CurrentLapTime;
			} else {
				_lapTime = _metrics.CurrentLapTime - _lastTime;
			}
			_lastTime = _metrics.CurrentLapTime;

            _opacity = 0;
            LapCount.text = "Checkpoint " + (checkpoint.CheckpointID)+"/"+(_metrics.NumberOfCheckpoints-1);
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
			InvokeRepeating("PassedCheckpointEndIn",0,1/60f);       
        }

        private float _opacity= 0;
        private TimeSpan _lapTime;
        private TimeSpan _lapLabelTime;//The label is what is display in the interface
        private void PassedCheckpointEndIn()
        {
        
            _opacity += (1/60f)/LapSummaryTimeIn;
        
            _lapLabelTime = new TimeSpan((long) Mathf.Lerp(TimeSpan.Zero.Ticks, _lapTime.Ticks,_opacity));
            if (_opacity >=1f)
            {
                _opacity = 1f;
                _lapLabelTime = _lapTime;
				CancelInvoke("PassedCheckpointEndIn"); 
				InvokeRepeating("PassedCheckpointEndOut", LapSummaryHoldTime, 1/60f);
            }
        }

        private void PassedCheckpointEndOut()
        {        
            _opacity -= (1 / 60f) / LapSummaryTimeOut;
            if (_opacity <=0f)
            {
                _opacity = 0;                             
                _lapLabelTime=TimeSpan.Zero;
                CancelInvoke("PassedCheckpointEndOut");
            }
        }


		public Color _UIColor;
// Update is called once per frame
        void Update () {
        
            if (_opacity >=0)
            {                
                transform.position = _rocketTransform.position;
                LapTime.text = StaticUtils.ToRaceTimeString(_lapLabelTime);
				Color opColor=Color.Lerp(Color.clear,this._UIColor,_opacity);
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

