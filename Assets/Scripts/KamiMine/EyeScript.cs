using UnityEngine;
using System.Collections;
namespace RealRocketRacing.Kamimine{
	public class EyeScript : MonoBehaviour {

		public float MaxOffset;
		private float _realMaxOffset;
		private Vector2 _offset;
		public float MinEyeSpeed;
		public float MaxEyeSpeed;

		public Transform Iris;

		private float _progress=1.1f;
		private Vector2 _target;
		private KamimineStateMachine _stateMachine;
		// Use this for initialization
		void Start () {
			_offset = Vector2.zero;
			_target = _offset;
			_stateMachine = GetComponentInParent<KamimineStateMachine> ();
		}
		
		// Update is called once per frame
		void Update () {
			_realMaxOffset = MaxOffset * transform.lossyScale.x;	
			if (_stateMachine.Rocket==null) {
				RandomEyeMovement ();
			} else {
				LookAt(_stateMachine.Rocket);

			}			
		}


		private void LookAt(GameObject rocket){
			var rocketPosition = rocket.transform.position;
			var delta = rocketPosition - transform.position;
			var normal = delta.normalized;
			var target = normal * _realMaxOffset+transform.position;
			var deltaEye = target - Iris.transform.position;
			if (deltaEye.sqrMagnitude > MaxEyeSpeed*MaxEyeSpeed) {
				Iris.transform.position+=normal*MaxEyeSpeed;
			} else {
				Iris.transform.position+=deltaEye;
			}
		}


		private float _delta;

		public float RefactoryTime;
		private float _currentRefactoryTime=1;
		private void RandomEyeMovement(){
			if (_progress >= 1) {//Assign next target
				_offset=_target;
				_target=Random.insideUnitCircle*_realMaxOffset;
				_progress=0;
				_currentRefactoryTime=Random.Range(-RefactoryTime,0);
				_delta=Time.deltaTime/Random.Range(MaxEyeSpeed,MinEyeSpeed);
			}
			if (_currentRefactoryTime > 0) {
				_progress += _delta;
				Iris.transform.localPosition = Vector2.Lerp (_offset, _target, _progress);
			} else {
				_currentRefactoryTime+=Time.deltaTime;
			}
		}

	}
}