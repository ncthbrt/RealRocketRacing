using UnityEngine;
using System.Collections;

public class DestoryObjectsWithTagClick : MonoBehaviour
{
    public GameObject[] Objects;
	// Use this for initialization
	void Start () {
	    
	}

    public void OnClick()
    {
        foreach (var o in Objects)
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag(o.tag))
            {                
                GameObject.Destroy(obj);
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
