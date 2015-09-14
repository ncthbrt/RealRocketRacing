using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;
using UnityEngine.UI;
using System;
namespace RealRocketRacing{
public class TimeSummaryScreenScript : MonoBehaviour {

	public Image Background;
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
		var color = rocket.GetComponent<SpriteRenderer> ().color;
		Background.color = color;
		textComponent.text = StaticUtils.ToRaceTimeString (metrics.TotalTime);
		Destroy (rocket);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}