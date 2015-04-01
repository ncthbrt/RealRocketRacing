using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class PhysicalParticle2D : MonoBehaviour
    {

        void Start()
        {
            gameObject.SetActive(false);
        }
    
        private float _decayRate;
        private float _health;
        private SpriteRenderer _renderer;
        public Color BaseColor;
        // Use this for initialization
        public void Reset(Vector2 velocity,Vector2 position,float rotation,float angularVelocity, float holdTime,float decayTime)
        {
            gameObject.SetActive(true);
            _renderer = GetComponent<SpriteRenderer>();

            _renderer.transform.position = Vector2.zero;
            var scale = Random.Range(0.7f, 1.4f);
            _renderer.transform.localScale = new Vector3(scale, scale, 1);
            rigidbody2D.position = position;
            rigidbody2D.velocity = velocity;
            rigidbody2D.angularVelocity = angularVelocity;

            _decayRate = Time.fixedDeltaTime/ decayTime;
            _health = 1f;
            _renderer = GetComponent<SpriteRenderer>();
            
            InvokeRepeating("Decay",holdTime,Time.fixedDeltaTime);        
        }

        public void Decay()
        {        
            _health -= _decayRate*Time.fixedDeltaTime;
            _health = _health > 0 ? _health : 0;
            _renderer.color=new Color(BaseColor.r*_health,BaseColor.g*_health,BaseColor.b*_health,BaseColor.a*_health);
            if (_health <=0)
            {                                
                gameObject.SetActive(false);
            }
        }
        
        public bool StillAlive { get { return _health > 0; } }
        
	
    }
}
