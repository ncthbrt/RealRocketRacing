using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointID;
	public Transform SpawnPoint;
	

    public Vector2 Location
    {
        get { return new Vector2(SpawnPoint.position.x, SpawnPoint.position.y); }
    }

    public float Heading
    {
        get { return SpawnPoint.eulerAngles.z; }
    }

    

}
