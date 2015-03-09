using System;
using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Console.WriteLine("Collided with trigger");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
