using UnityEngine;
using System.Collections;
namespace RealRocketRacing.Kamimine{
	public class TriggerZone : MonoBehaviour {
		void OnTriggerEnter2D(Collider2D other){
			if (other.tag.Equals ("Player")) {			
				_currentScale=Mathf.Lerp (_currentScale, BreatheInScale, _progress);
				CancelInvoke("Expand");

                if (_stateMachine.State != KamimineState.Charging && !TriggerSound.isPlaying)
                {
                    TriggerSound.Play();
                }
                _stateMachine.SensorTriggerred(other.gameObject);
                
			    
			}
		}

	    public AudioSource TriggerSound;

		// Use this for initialization
		void Start () {
			_delta = 2f*Time.fixedDeltaTime / BreathePeriod;
			_stateMachine = GetComponentInParent<KamimineStateMachine> ();
			_stateMachine.AddStateChangeListener (StateChanged);
			_renderer = GetComponent<SpriteRenderer> ();
			_renderer.color = UntrigerredColor;
			InvokeRepeating ("Breathe", 0,Time.fixedDeltaTime * 2);
		}
		private SpriteRenderer _renderer;

		public void StateChanged(KamimineState state){
			if (state == KamimineState.Charging) {
				_progress = 0;
				CancelInvoke ("Breathe");
				_delta = Time.fixedDeltaTime * 2f / (_stateMachine.ChargingTime * 0.9f);
				_renderer.color = TriggerredColor;
				InvokeRepeating ("Contract", _stateMachine.ChargingTime * 0.1f, Time.fixedDeltaTime * 2f);
			} else if (state == KamimineState.Attacking) {
				gameObject.SetActive (false);
			} else if (state == KamimineState.Searching) {
				gameObject.SetActive(true);
				_renderer.color=UntrigerredColor;
				_progress=0;
				InvokeRepeating ("Expand", 0, Time.fixedDeltaTime * 2f);
			}
		}

		public void Expand(){
			_progress += _delta;					
			float scale;
			if (_progress >= 1) {
				CancelInvoke ("Expand");
				InvokeRepeating ("Breathe", 0, Time.fixedDeltaTime * 2);
				_progress = 0f;
				scale = BreatheInScale;
			} else{
				scale= Mathf.Lerp (_currentScale, BreatheInScale, _progress);
			}
			transform.localScale=new Vector3(scale,scale,1);
		}



		private KamimineStateMachine _stateMachine;
		private float _delta;
		private float _progress;
		private float _currentScale;

		void Breathe(){
			_progress += _delta;
			_progress %= 1;
			_currentScale = Mathf.Lerp (BreatheInScale, BreatheOutScale, Mathf.Sin (Mathf.Lerp(0,Mathf.PI*2,_progress)));
	        transform.localScale=new Vector3(_currentScale,_currentScale,1);
		}

		void Contract(){
			_progress += _delta;
			_progress = _progress > 1 ? 1f : _progress;	
			var scale = Mathf.Lerp (_currentScale, ContractFinalScale, _progress);
			if (_progress >= 1) {
				_currentScale=scale;
				CancelInvoke("Contract");
			}

			transform.localScale=new Vector3(scale,scale,1);
		}
		public float BreatheOutScale=2f;
		public float BreatheInScale=1.8f;
		public float ContractFinalScale=0.3f;
		public float BreathePeriod=4;
		public Color UntrigerredColor;
		public Color TriggerredColor;

	}
}