﻿using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;

namespace RealRocketRacing.Kamimine{
	public class KamimineChargingAnimation : MonoBehaviour {

		public float MaxShakeIntensity;

		public float FullyChargedScale=2f;
		public float ExhaustedChargeScale = 1f;

		private KamimineStateMachine _stateMachine;		
		public SpriteRenderer[] KamiminePartRenderers;
		public Transform Sensor;
	    public AudioSource ChargingAudio;
        public AudioSource DischargingAudio;
        private float _chargingDelta;
		private float _dischargingDelta;
		private float _transitionProgress;

		private float _dischargeRatioToAttackTime=0.2f;
		// Use this for initialization
		void Start () {
			_stateMachine = GetComponent<KamimineStateMachine> ();
			_stateMachine.AddStateChangeListener (StartCharge);
			ResetKamimine ();
			_chargingDelta=Time.fixedDeltaTime/_stateMachine.ChargingTime;
			_dischargingDelta = Time.fixedDeltaTime / (_stateMachine.AttackBurstTime * _dischargeRatioToAttackTime);
		}

		private void ResetKamimine(){
			transform.localScale = new Vector3(ExhaustedChargeScale,ExhaustedChargeScale,1);
			foreach (var renderer in KamiminePartRenderers) {
				renderer.color=GetComponent<RocketPalette>().BaseColor;
			}
		}


		private void StartCharge(KamimineState state){
			if (state == KamimineState.Charging) {
				CancelInvoke ("Discharge");
				_transitionProgress = 0;
                ChargingAudio.Play();
                InvokeRepeating ("Charge", 0, Time.fixedDeltaTime);				
			} else if (state == KamimineState.Reviving) {
				ResetKamimine();
			}
			else {
				Sensor.localPosition=Vector3.zero;
				Sensor.localRotation=Quaternion.identity;
				CancelInvoke("Charge");

                if (state==KamimineState.Attacking){
                    DischargingAudio.Play();
                    InvokeRepeating("Discharge",(1f-_dischargeRatioToAttackTime)*_stateMachine.AttackBurstTime,Time.fixedDeltaTime);
				}
			}
		}

		private void Discharge(){
			_transitionProgress -= _dischargingDelta;
			if(_transitionProgress<0){
				_transitionProgress=0;
			}
			var color=Color.Lerp (GetComponent<RocketPalette>().BaseColor, GetComponent<RocketPalette>().AccentColor, _transitionProgress);
			var scale = Mathf.Lerp (ExhaustedChargeScale, FullyChargedScale, _transitionProgress);
			transform.localScale = new Vector3(scale,scale,1);

			foreach (var renderer in KamiminePartRenderers) {
				renderer.color=color;
			}
		}



		private void Charge(){
			_transitionProgress += _chargingDelta;
			if (_transitionProgress > 1) {
				_transitionProgress=1f;
			}
			var shakeIntensity = _transitionProgress * MaxShakeIntensity;
			var color=Color.Lerp (GetComponent<RocketPalette>().BaseColor, GetComponent<RocketPalette>().AccentColor, _transitionProgress);

			var scale = Mathf.Lerp (ExhaustedChargeScale, FullyChargedScale, _transitionProgress);
			transform.localScale = new Vector3(scale,scale,1);

			foreach (var renderer in KamiminePartRenderers) {
				renderer.color=color;
			}
			var offset = Random.insideUnitCircle * shakeIntensity;

			GetComponent<Rigidbody2D>().position = GetComponent<Rigidbody2D>().position + offset;
			Sensor.localPosition=-offset;
			var angleOffset = Random.Range (-shakeIntensity, shakeIntensity) * .2f;

			transform.rotation= Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().rotation + angleOffset);
			Sensor.localEulerAngles+=new Vector3(0, 0, angleOffset);
		}
			
	}
}