using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RealRocketRacing.RRRCamera;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.Hud{
	public class RaceStartRenderer : MonoBehaviour {
		

		public Canvas Hud;
		public CameraScript MainCameraScript;
		public RocketController Controller;

		public Image[] Numbers;
		public RocketRaceMetrics Metrics;
		private int _number=0;


		public Image Go;
		private Color _goColor;

		private Color[] _numberColors;

		void Start () {
			var fillAmount = 1f / Numbers.Length;
			_numberColors = new Color[Numbers.Length];
			for (int i=0; i<Numbers.Length; ++i) {
				var offset=Numbers[i].rectTransform.rect.width*fillAmount*i;
				Numbers[i].rectTransform.anchoredPosition=new Vector2(offset,0);
				_numberColors[i]=Numbers[i].color;
				Numbers[i].color=Color.clear;
			}
			_goColor = Go.color;
			Go.color = Color.clear;

			_numberOpacity = 0f;
			_number = -1;
			Hud.enabled = false;
			Controller.ControlsEnabled = false;
			InvokeRepeating ("NextNumber", 2f, 0.5f);
		}
		private float _numberOpacity;

		// Update is called once per frame
		private void FixedUpdate () {
			if (_number >= 0) {
				_numberOpacity += Time.fixedDeltaTime * 2f;
				if (_number < Numbers.Length) {			

					Numbers [_number].color = Color.Lerp (Color.clear, _numberColors [_number], Mathf.Sin (Mathf.Lerp (0, Mathf.PI / 2f, _numberOpacity)));
				} else {
					var nextGoColor = Color.Lerp (Color.clear, _goColor, Mathf.Sin (Mathf.Lerp (0, Mathf.PI, _numberOpacity)));
					Go.color = nextGoColor;

				}
			}
		}
		
		public AudioSource Beep321;
		public AudioSource BeepGo;
		private void NextNumber(){
			_number++;			
			_numberOpacity=0;
			if (_number >= Numbers.Length) {
				CancelInvoke ("NextNumber");
				Invoke ("PlayBeepGo", 0.25f);
				for (int i=0; i<Numbers.Length; ++i) {
					Numbers [i].color = Color.clear;
				}
				Invoke ("StartRace", 0.5f);
			} else {
				Beep321.Play();
			}
		}


		private void PlayBeepGo(){
			BeepGo.Play();
			MainCameraScript.StartShake(0.5f,0.7f);
		}

		public AudioSource SoundTrack;
		private void StartRace(){
			Controller.ControlsEnabled = true;
			Hud.enabled=true;
			Metrics.StartTimer();
			SoundTrack.Play ();	

			this.enabled=false;
		}
	}
}