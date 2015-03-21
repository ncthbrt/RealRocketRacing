using System;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public Rigidbody2D Rocket;

    private void Start()
    {    
    }



	void FixedUpdate ()
	{
        Vector2 delta = Rocket.position - new Vector2(transform.position.x, transform.position.y);            
        transform.Translate(delta);	    
	}
}
