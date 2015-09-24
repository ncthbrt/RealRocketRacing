using UnityEngine;
using System.Collections;
using System;
using RealRocketRacing.RaceCheckpoints;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.Hud{
public class SoundEffectSystem : MonoBehaviour {

		public AudioSource Damage;
		public AudioSource Explosion;
		public AudioSource PassedCheckpoint;
		public AudioSource Thrust;
		public AudioSource DirectionalThrust;
		public AudioSource LapComplete;

		public RocketDamageSystem DamageSystem;
		public RocketController Controller;
		public RocketRaceMetrics Metrics;


		void Start () {


			Metrics.AddPassedCheckpointCallback (PlayPassedCheckpointSound);
			Metrics.AddLapCompleteCallback (PlayLapCompleteSound);
			DamageSystem.AddOnDamageCallback (PlayDamageSound);
			DamageSystem.AddRespawnCallback (PlayExplosionSound);
			Controller.AddPrimaryThurstOnCallback (StartThrustNoise);
			Controller.AddPrimaryThurstOffCallback (StopThrustNoise);
			Controller.AddLeftThrustOnCallback (StartDirectionalThrustNoise);
			Controller.AddLeftThrustOffCallback (StopDirectionalThrustNoise);
			Controller.AddRightThrustOnCallback (StartDirectionalThrustNoise);
			Controller.AddRightThrustOffCallback (StopDirectionalThrustNoise);
			Thrust.loop = true;
			DirectionalThrust.loop = true;
		}

		private void StartDirectionalThrustNoise(){
			DirectionalThrust.Play ();
		}
		private void StopDirectionalThrustNoise(){
			DirectionalThrust.Stop();
		}

		private void StartThrustNoise(){
			Thrust.Play();
		}

		private void StopThrustNoise(){
			Thrust.Pause ();
		}

		private void PlayPassedCheckpointSound(Checkpoint checkpoint, GameObject rocket){
			if (checkpoint.CheckpointID != 0) {
				PassedCheckpoint.Play ();
			}
		}



		private void PlayLapCompleteSound(GameObject rocket, TimeSpan lapTime,int lapNumber){
			LapComplete.Play();
		}



		private void PlayDamageSound(GameObject rocket, float damage,float remainingHealth){
			Damage.Play ();
		}

		private void PlayExplosionSound(GameObject rocket,Vector2 position){
			Explosion.Play();
		}
	}
}