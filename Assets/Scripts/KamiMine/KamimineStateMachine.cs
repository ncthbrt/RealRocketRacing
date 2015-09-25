using UnityEngine;
using System.Collections;
namespace RealRocketRacing.Kamimine{
public class KamimineStateMachine : MonoBehaviour {
		private Vector2 _startingPosition;
		private float _startingRotation;
		public float RespawnTime;

		public void Reset(){
			Rocket = null;			
			_periodCount = 0;
			State = KamimineState.Searching;            
		}


		private GameObject _rocket;



		private KamimineState _state=KamimineState.Searching;
		public float PeriodsBeforeForget=3;
		public float ChargingTime;
		public float RechargingRefactoryTime;
		public float OpeningTime;
		public float ClosingTime;
		public float AttackBurstTime;
		private int _periodCount = 0;
		public KamimineState State{
			get{
				return _state;
			}
			set{
				if(_state!=value){
					if(_listeners!=null){
						_listeners(value);
					}
					if(value==KamimineState.Destroyed){
						gameObject.SetActive(false);                        
                        Invoke("ToRevival",RespawnTime);
					}else if(value==RealRocketRacing.Kamimine.KamimineState.Reviving){
						gameObject.SetActive(true);                        
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
						GetComponent<Rigidbody2D>().angularVelocity = 0;						
						GetComponent<Rigidbody2D>().position = _startingPosition;                        
                        GetComponent<Rigidbody2D>().rotation = _startingRotation;
                        _periodCount = 0;                        					
					}
		         	else
					{					    
                        KamimineAnimator.SetInteger("KamimineState",(int)value);
						_state=value;
						switch(_state){
							case KamimineState.Charging:
								Invoke ("ToEyesClosing",ChargingTime);
							break;
							case KamimineState.EyesClosing:
								Invoke ("ToAttack",ClosingTime);
							break;
							case KamimineState.Attacking:
								Invoke("ToEyesOpening",AttackBurstTime);
							break;
							case KamimineState.EyesOpening:
								Invoke ("ToWatching",OpeningTime);
							break;
							case KamimineState.Watching:							
									++_periodCount;
									if(_periodCount>=PeriodsBeforeForget){
										Reset ();
									}else{
										Invoke ("ToCharging",RechargingRefactoryTime);
									}							
							break;
						}
					}
				}

			}
		}

		public GameObject Rocket{ 
			get{
				if(State!=KamimineState.Watching && State!=KamimineState.Searching){
					return _rocket;
				}else{
					return null;
				}
			} 
			private set{
				_rocket=value;
			}
		}

		public void SensorTriggerred(GameObject rocket){
			if (State != KamimineState.Charging){
				Rocket=rocket;
 				State = KamimineState.Charging;
			}
		}

		private void ToRevival(){
			State = KamimineState.Reviving;            
        }
        private void ToEyesClosing(){
			State = KamimineState.EyesClosing;
		}


		private void ToCharging(){
			State = KamimineState.Charging;
		}
		private void ToEyesOpening(){
			State = KamimineState.EyesOpening;
		}
		private void ToAttack(){
				State=KamimineState.Attacking;				
		}

		private void ToWatching(){
			State = KamimineState.Watching;
		}
		public delegate void StateChangeListener(KamimineState state);
		private StateChangeListener _listeners;

		public void AddStateChangeListener(StateChangeListener listener){
			_listeners += listener;
		}


		private Animator KamimineAnimator;

		// Use this for initialization
		void Start () {
			KamimineAnimator = GetComponentInChildren<Animator> ();
			_startingPosition = GetComponent<Rigidbody2D>().position;
			_startingRotation=GetComponent<Rigidbody2D>().rotation;

		}
	
	}
}