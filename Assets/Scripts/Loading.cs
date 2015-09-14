using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
			Invoke("Load", 1);
	}

	void Load(){
		Application.LoadLevel ("Industry");
	}
}
