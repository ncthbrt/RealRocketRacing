using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class RocketController : MonoBehaviour {

    public GameObject[] MainThrusters;
    public GameObject[] LeftThrusters;
    public GameObject[] RightThrusters;
	// Use this for initialization

    private bool _mainThrusterOn=false;
    private bool _leftThrusterOn=false;
    private bool _rightThrusterOn=false;

    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update ()
	{

	    bool lastMain = _mainThrusterOn;
	    bool lastLeft = _leftThrusterOn;
	    bool lastRight = _rightThrusterOn;

        HandleControllerInput();       

	    if (lastMain^_mainThrusterOn)
	    {         
	        ThrusterStateChange(_mainThrusterOn,MainThrusters);
	    }
        if (lastLeft^ _leftThrusterOn)
        {         
            ThrusterStateChange(_leftThrusterOn, LeftThrusters);
        }
        if (lastRight^ _rightThrusterOn)
        {         
            ThrusterStateChange(_rightThrusterOn, RightThrusters);
        }        
	}

    void HandleControllerInput()
    {
        if (!_mainThrusterOn && Input.GetKey("up"))
        {
            _mainThrusterOn = true;
        }
        else if (_mainThrusterOn && !Input.GetKey("up"))
        {
            _mainThrusterOn = false;            
        }


        if (!_leftThrusterOn && Input.GetKey("right"))
        {
            _leftThrusterOn = true;
        }
        else if (_leftThrusterOn && !Input.GetKey("right"))
        {
            _leftThrusterOn = false;            
        }

        if (!_rightThrusterOn && Input.GetKey("left"))
        {
            _rightThrusterOn = true;            
        }
        else if (_rightThrusterOn && !Input.GetKey("left"))
        {
            _rightThrusterOn = false;            
        }

        if (Input.GetKey("r"))
        {

			var r_system=GetComponent<RocketRaceMetrics>();
            rigidbody2D.velocity=new Vector2();
            rigidbody2D.angularVelocity =0 ;
            rigidbody2D.MovePosition(r_system.CurrentCheckpoint.Location);
			transform.eulerAngles=new Vector3(0,0,r_system.CurrentCheckpoint.Heading);
        }
    }


    private void ThrusterStateChange(bool on, IEnumerable<GameObject> thrusters)
    {        
        foreach (var thruster in thrusters)
        {
            var renderScript = thruster.GetComponent<AbstractThrusterVisuals>();
            if (renderScript != null)
            {
                if (on)
                {
                    renderScript.ThrusterOn();
                }
                else
                {
                    renderScript.ThrusterOff();
                }
            }
        }
    }
    
    private void ApplyThrust(IEnumerable<GameObject> thrusters)
    {
        foreach (var thruster in thrusters)
        {            
            ThrusterPhysics thrusterPhysicsScript = thruster.GetComponent<ThrusterPhysics>();
            if (thrusterPhysicsScript != null)
            {
                thrusterPhysicsScript.ApplyThrust(rigidbody2D);
            }
        }        
    }
    void FixedUpdate()
    {
        if (_mainThrusterOn)
        {         
            ApplyThrust(MainThrusters);
        }
        if (_leftThrusterOn)
        {         
            ApplyThrust(LeftThrusters);
        }
        if (_rightThrusterOn)
        {         
            ApplyThrust(RightThrusters);
        }                
    }
}

