using UnityEngine;
using System.Collections;

public class SountrackSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    GameObject.DontDestroyOnLoad(this.gameObject);
	}   	
}
