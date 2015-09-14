using UnityEngine;
using System.Collections;
namespace RealRocketRacing.Kamimine{
	public class KamimineAttackScript : MonoBehaviour {
		private KamimineStateMachine _stateMachine;
		public float Force=3;

		// Use this for initialization
		void Start () {
			_stateMachine = GetComponent<KamimineStateMachine> ();
			_stateMachine.AddStateChangeListener (StateChange);
		}


		Vector2 direction;
		private void StateChange(KamimineState state){
			if (state == KamimineState.Attacking) {
				CancelInvoke("Decellerate");
				InvokeRepeating("Chase",0,Time.fixedDeltaTime);
					
			} else {
				CancelInvoke("Chase");
				InvokeRepeating("Decellerate",0,Time.fixedDeltaTime);
			}
		}
		public float VelocityThreshold = 0.001f;
		private void Decellerate(){
			GetComponent<Rigidbody2D>().velocity *= 0.9f;
			if (GetComponent<Rigidbody2D>().velocity.magnitude < VelocityThreshold) {
				GetComponent<Rigidbody2D>().velocity=Vector2.zero;
			}
		}

		private void Chase(){
			if (_stateMachine.Rocket != null) {
				var rocketRigidBody = _stateMachine.Rocket.GetComponent<Rigidbody2D> ();
				var rocketPosition = rocketRigidBody.position;
				var rocketVelocity = rocketRigidBody.velocity;
				var deltaPosition = rocketPosition - GetComponent<Rigidbody2D>().position;
				var distance = deltaPosition.magnitude;
				var relativeVelocity = GetComponent<Rigidbody2D>().velocity - rocketVelocity;
				if (relativeVelocity.magnitude != 0) {
					var timeToClose = distance / (relativeVelocity.magnitude);			
					var predictedPosition = rocketPosition + rocketVelocity.normalized * timeToClose;
					var deltaPredictedForce = (GetComponent<Rigidbody2D>().position - predictedPosition).normalized * Force * Time.fixedDeltaTime;
					GetComponent<Rigidbody2D>().AddForce (-deltaPredictedForce, ForceMode2D.Impulse);
				}
			} else {
				CancelInvoke("Chase");

			}
		}
		
	}
}