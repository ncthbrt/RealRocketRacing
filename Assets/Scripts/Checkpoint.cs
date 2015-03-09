using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointID;
    public Vector2 Location
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    public float Heading
    {
        get { return transform.eulerAngles.z; }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
