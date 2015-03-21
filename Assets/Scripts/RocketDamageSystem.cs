using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketDamageSystem : MonoBehaviour {
	private RocketCheckpointSystem _checkpointSystem;
	// Use this for initialization
	void Start () {
		_checkpointSystem = GetComponent<RocketCheckpointSystem> ();
	}

	// Update is called once per frame
	void Update () {
	
	}


	public float MaxImpulseForce;
	
	private IList<Collision2D> collisions=new List<Collision2D>();
	private IList<Vector2> initialVelocity=new List<Vector2>();
	private IList<float> collisionTime=new List<float>();


	public void OnCollisionEnter2D(Collision2D other){
		var currentTime = Time.time;
		collisions.Add (other);
		initialVelocity.Add (rigidbody2D.velocity);	
		collisionTime.Add (currentTime);
	}
	

	private void ResolveCollision(Collision2D other){
		for (int i=0; i<collisions.Count; ++i) {
			if(collisions[i].collider==other.collider){//Colliding w/ same object					
				var deltaTime=Time.time-collisionTime[i];
				var deltaVelocity=rigidbody2D.velocity - initialVelocity[i];
				var impulseForce=rigidbody2D.mass*deltaVelocity/deltaTime;
				if(impulseForce.sqrMagnitude>MaxImpulseForce*MaxImpulseForce){
					Respawn();
				}
				collisions.RemoveAt(i);
				collisionTime.RemoveAt(i);
				initialVelocity.RemoveAt(i);
				break;
			}
		}
	}


	public void OnCollisionExit2D(Collision2D other){
		ResolveCollision(other);		
	}

	
	public void Reset()
	{

	}
}
