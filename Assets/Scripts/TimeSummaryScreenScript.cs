using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;
using System;
namespace RealRocketRacing{
public class TimeSummaryScreenScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
		var textComponent=GetComponent<UnityEngine.UI.Text> ();
		var rocket=GameObject.Find ("rocket");
		var metrics = rocket.GetComponent<RocketRaceMetrics> ();
			var bestLapTime = TimeSpan.MaxValue;
			foreach (var time in metrics.LapTimes) {
				if (time<bestLapTime){
					bestLapTime=time;
				}
			}
		textComponent.text = "Your time: " + StaticUtils.ToRaceTimeString (metrics.TotalTime) + "\n\n"
				+ "Your best lap time was: " + StaticUtils.ToRaceTimeString (bestLapTime);

		Destroy (rocket);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}