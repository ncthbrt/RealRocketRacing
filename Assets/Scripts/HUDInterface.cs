using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HUDInterface : MonoBehaviour
    {


        public Text LapCountText;

        public Text LapTimeText;

        public Text TotalTimeText;

        public RocketRaceMetrics RaceMetrics;



        // Use this for initialization
        private void Start()
        {
        
            RaceMetrics.AddLapCompleteCallback(LapComplete);
            LapCountText.text = (RaceMetrics.LapCount + 1) + "/" + RaceMetrics.NumberOfLaps;
        }

        void LapComplete(GameObject rocket, TimeSpan lapTime, int lapNumber)
        {
            LapCountText.text= (RaceMetrics.LapCount+2)+"/"+RaceMetrics.NumberOfLaps;
        }

        // Update is called once per frame
        private void Update()
        {            
            TotalTimeText.text = StaticUtils.ToRaceTimeString(RaceMetrics.TotalTime);
            LapTimeText.text = StaticUtils.ToRaceTimeString(RaceMetrics.CurrentLapTime);            
        }


    }
}
