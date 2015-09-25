using UnityEngine;
using System.Collections;
using RealRocketRacing.ParticleSystem;

namespace RealRocketRacing.Kamimine{
	public class KamimineDamage : MonoBehaviour
	{

	    public AudioSource KamimineExplosionSound;

		private KamimineStateMachine _stateMachine;
		private ExplosionParticleSystem _explosionSystem;
		// Use this for initialization
		void Start () {		

			_explosionSystem = GetComponent<ExplosionParticleSystem> ();
			_stateMachine = GetComponent<KamimineStateMachine> ();
		}



		public void OnCollisionEnter2D(Collision2D other)
		{
			if(!other.gameObject.tag.Equals("Effects")){
				DestroyKamimine();
			}
		}	
		private void DestroyKamimine(){
            KamimineExplosionSound.Play();
			_explosionSystem.StartExplosion (GetComponent<Rigidbody2D>().velocity, GetComponent<Rigidbody2D>().position);
			_stateMachine.State = KamimineState.Destroyed;	
		}
	}
}
