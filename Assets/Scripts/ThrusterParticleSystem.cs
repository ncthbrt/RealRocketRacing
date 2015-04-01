using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ThrusterParticleSystem : MonoBehaviour
    {
        private IList<PhysicalParticle2D> _particles;

        public Transform LeftEdge;
        public Transform RightEdge;

        public RocketController Controller;
        public Rigidbody2D RocketRigidbody2D;
        public float Rate;
        public float DecayTime;
        
        public PhysicalParticle2D Template;
        public float AngularVelocity = 0f;        
        public float AdditionalVelocity = 4f;

        private int _currentParticle = 0;
        // Use this for initialization
        void Start () {
	        _particles=new PhysicalParticle2D[Mathf.CeilToInt(Rate*DecayTime)];                                 
            Controller.AddPrimaryThurstOnCallback(PrimaryThrusterOn);
            Controller.AddPrimaryThurstOffCallback(PrimaryThrusterOff);
            for (int i = 0; i < _particles.Count; ++i)
            {
                _particles[i] = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;                
            }
        }

        private bool _thrust= false;

        private void PrimaryThrusterOn()
        {
            
            InvokeRepeating("EmitParticle", 0, 1f / Rate);
        }



        private void PrimaryThrusterOff()
        {
            
            CancelInvoke("EmitParticle");
        }




        private void EmitParticle()
        {
            var start = Vector2.Lerp(LeftEdge.position, RightEdge.position, Random.Range(0.1f,0.9f));

            var velocity = RocketRigidbody2D.velocity;
            var angle = (RocketRigidbody2D.rotation - 90f) / 180f * (Mathf.PI) + Random.Range(0, Mathf.PI * 0.6f) - Mathf.PI * 0.3f;
            velocity+= (new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)) * AdditionalVelocity);
           
            
            _particles[_currentParticle].Reset(velocity, start, 0, 0, 0, DecayTime);
            ++_currentParticle;
            _currentParticle = _currentParticle%_particles.Count;
        }
        
        // Update is called once per frame
        void Update ()
        {            
            
        }
    }
}
