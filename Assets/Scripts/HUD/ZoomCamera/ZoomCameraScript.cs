using UnityEngine;
using System.Collections;

public class ZoomCameraScript : MonoBehaviour
{
    public GameObject Rocket;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = new Vector3(Rocket.transform.position.x,Rocket.transform.position.y,transform.position.z);
	}
}
