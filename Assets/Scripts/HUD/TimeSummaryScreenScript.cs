using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;
using UnityEngine.UI;
using System;
namespace RealRocketRacing{
public class TimeSummaryScreenScript : MonoBehaviour
{

    public GameObject Rocket;
    public float VelocityX;
    public float VelocityY;
        // Use this for initialization
        void Start () {
		var textComponent=GetComponent<UnityEngine.UI.Text> ();
		var info=GameObject.FindGameObjectWithTag("VictoryInfo");
		var metrics = info.GetComponent<VictoryInfo> ();
	    var colors = info.GetComponent<RocketPalette>();
	    var pallete=Rocket.GetComponent<RocketPalette>();
        var controller = Rocket.GetComponent<RocketController>();
        controller.RocketPlayer = metrics.WinningPlayer;         
        controller.Reset();

        var rigid = Rocket.GetComponent<Rigidbody2D>();
        Debug.Log(metrics.Velocity);
        rigid.velocity = new Vector2(metrics.Velocity.x,metrics.Velocity.y);
        rigid.rotation= metrics.Rotation;
	    pallete.BaseColor = colors.BaseColor;
	    pallete.AccentColor = colors.AccentColor;
        pallete.ResetColors();        
		textComponent.text = StaticUtils.ToRaceTimeString (metrics.Time);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}