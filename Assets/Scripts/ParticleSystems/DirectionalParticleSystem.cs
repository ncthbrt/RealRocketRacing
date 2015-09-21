using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RealRocketRacing.Rocket;

namespace RealRocketRacing.ParticleSystem{
	public class DirectionalParticleSystem : MonoBehaviour {
		private IList<PhysicalParticle2D> _particles;
		
		private Color _startColor,_endColor;
		public RectTransform Emmitter;
		public float Rate;
		public float DecayTime;
		public float HoldTime=0;        
		public PhysicalParticle2D Template;
		public float AngularVelocity = 0f;        
		public float Direction;
		public float Spread;
		public float Velocity = 4f;

		public bool On=true;
		// Use this for initialization
		void Start ()
		{
		        var palette = GetComponent<RocketPalette>();
		        _startColor = palette.BaseColor;
		        _endColor = palette.AccentColor;

				_particles=new PhysicalParticle2D[Mathf.CeilToInt(Rate*DecayTime)];		
				for (int i = 0; i < _particles.Count; ++i)
				{
					_particles[i] = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;                
				}

			if(On){
				TurnOn();
			}
		}

		public void TurnOn(){
			gameObject.SetActive (true);
			InvokeRepeating("EmitParticle", 0, 1f / Rate);
			

		}
		private int _currentParticle=0;
		private void EmitParticle()
		{
			var corners = new Vector3[4];
			Emmitter.GetWorldCorners(corners);
			Vector3 left = corners [0];
			Vector3 right= corners[2];
			var start = Vector2.Lerp(right,left,Random.Range(0.2f,0.8f));			

			var angle = Direction/180f*Mathf.PI;
			var velocity= (new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)) * Random.value*Velocity);
			
			_particles[_currentParticle++].Reset(velocity,start, 0,0, HoldTime, DecayTime,_startColor,_endColor);
			_currentParticle = _currentParticle%_particles.Count;            
		}

		public void TurnOff(){
			gameObject.SetActive (false);
		}

		// Update is called once per frame
		void Update () {

		}
	}
}