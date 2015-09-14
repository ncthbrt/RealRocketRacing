using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;

public class RegistryStartupHack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<RocketRegistry> ().Init (2);
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
