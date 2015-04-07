using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace RealRocketRacing.ParticleSystem{
	public class ExplosionParticleSystem : MonoBehaviour {

		private IList<PhysicalParticle2D> _particles;

		public float Intensity;
		public float Falloff;
		public PhysicalParticle2D Template;
		public float ParticleVelocity;
		// Use this for initialization
		void Start () {
			Setup ();
		}

		public void Setup(){
			_particles=new PhysicalParticle2D[Mathf.CeilToInt(Intensity*MaxParticleDecayTime)];
			for (int i = 0; i < _particles.Count; ++i)
			{
				_particles[i] = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;
			}
			_particleNo = 0;
		}

		
		private float _spawnRate;
		private float _delta;
		private int _particleNo = 0;
		private float _varience;
		private Vector2 _velocity;
		private Vector2 _spawnPoint;
		public float PositionVarience;
		public float VelocityVarience;
		public float MaxParticleDecayTime;
		public float ParticleHoldTime;
		public float MinParticleDecayTime;
		public float MaxAngularVelocity;

		public void StartExplosion(Vector2 velocity, Vector2 spawnPoint){
			gameObject.SetActive (true);
			_spawnRate = Intensity;
			_velocity = velocity;
			_spawnPoint = spawnPoint;
			_bag = 0;
			InvokeRepeating("Explode",0,Time.fixedDeltaTime);
		}

		private float _bag=0;
		private void Explode(){
			_spawnRate -= (Time.fixedDeltaTime*Falloff);
			float intermediateCount = (_spawnRate * Time.fixedDeltaTime);
			_bag += intermediateCount % 1f;

			int spawnCount = (int)intermediateCount;
			if (_bag >= 1f) {
				_bag-=1f;
				++spawnCount;
			}
			if (_spawnRate < 0) {
				CancelInvoke ("Explode");
			} else {
				for(int i=0; i<spawnCount; ++i){
					var velocity = _velocity+ ParticleVelocity*Random.insideUnitCircle;
					_particles[_particleNo++].Reset(velocity,_spawnPoint+Random.insideUnitCircle*Random.value*PositionVarience,360*Random.value,Random.value*MaxAngularVelocity*2-MaxAngularVelocity,ParticleHoldTime,Random.Range(MinParticleDecayTime,MaxParticleDecayTime));
					_particleNo=_particleNo%_particles.Count;
				}
			}
		}

	}
}