using UnityEngine;
using System.Collections;

public class DestoryObjectsWithTagClick : MonoBehaviour
{
    public GameObject Object;
	// Use this for initialization
	void Start () {
	    
	}

    public void OnClick()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag(Object.tag))
        {
            Debug.Log("Destroying");
            GameObject.Destroy(obj);
        }                
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
