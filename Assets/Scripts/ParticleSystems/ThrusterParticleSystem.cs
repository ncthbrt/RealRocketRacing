using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.ParticleSystem
{
    public class ThrusterParticleSystem : MonoBehaviour
    {
        private IList<PhysicalParticle2D> _particles;

        
        public Transform LeftEdge;
        public Transform RightEdge;

        public RocketController Controller;
        public RocketController.ThrusterGroup ThrusterGroup;
        public Rigidbody2D RocketRigidbody2D;
        public float Rate;
        public float DecayTime;
        public float HoldTime=0;        
        public PhysicalParticle2D Template;
        public float AngularVelocity = 0f;        
        public float AdditionalVelocity = 4f;
		public float MaxAngle=0f;
		public float _maxAngleRads=0f;

        private int _currentParticle = 0;
        // Use this for initialization
        void Start () {
			_maxAngleRads = MaxAngle / 180 * Mathf.PI;
	        _particles=new PhysicalParticle2D[Mathf.CeilToInt(Rate*DecayTime)];
            switch (ThrusterGroup)
            {
                    case RocketController.ThrusterGroup.PRIMARY:
                        Controller.AddPrimaryThurstOnCallback(On);
                        Controller.AddPrimaryThurstOffCallback(Off);
                    break;
                    case RocketController.ThrusterGroup.LEFT:
                        Controller.AddLeftThrustOnCallback(On);
                        Controller.AddLeftThrustOffCallback(Off);
                    break;
                    case RocketController.ThrusterGroup.RIGHT:
                        Controller.AddRightThrustOnCallback(On);
                        Controller.AddRightThrustOffCallback(Off);
                    break;
            }
            
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i] = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;                
            }
        }

        

        

        private void On()
        {
            InvokeRepeating("EmitParticle", 0, 1f / Rate);
        }


        private void Off()
        {
            CancelInvoke("EmitParticle");
        }






        private void EmitParticle()
        {
            var start = Vector2.Lerp(LeftEdge.position, RightEdge.position, Random.Range(0.2f,0.8f));

            var velocity = RocketRigidbody2D.velocity;
            var angle = (RocketRigidbody2D.rotation - 90f) / 180f * (Mathf.PI) + Random.Range(0, _maxAngleRads) - Mathf.PI * _maxAngleRads/2f;
            velocity+= (new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)) * AdditionalVelocity);
                       
            _particles[_currentParticle++].Reset(velocity,start, Random.Range(0,180),Random.Range(0,10)-5, HoldTime, DecayTime);

            _currentParticle = _currentParticle%_particles.Count;            
        }

    }
}
