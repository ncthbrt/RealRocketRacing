using System;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public Rigidbody2D Rocket;
    public float MaxSpeed;
    public float Mass;
        
    
    public float DeadZone;    

    private void Start()
    {    
    }



	void FixedUpdate ()
	{
        Vector2 delta = Rocket.position - new Vector2(transform.position.x, transform.position.y);            
        transform.Translate(delta);	    
	}
}
