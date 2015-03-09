using UnityEngine;
using System.Collections;

public abstract class AbstractThrusterVisuals : MonoBehaviour {

	// Use this for initialization
    public abstract void Start();
	
	// Update is called once per frame
    public abstract void Update();

    public abstract void ThrusterOn();
    public abstract void ThrusterOff();
}
