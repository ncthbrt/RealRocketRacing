using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RealRocketRacing.ParticleSystem
{
    public class PhysicalParticle2D : MonoBehaviour
    {

        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            var material = new Material(Shader.Find("Particles/Additive"));
            _renderer.material = material;
            gameObject.SetActive(false);
        }
    
        private float _decayRate;
        private float _health;
        private SpriteRenderer _renderer;
        public Color StartColor;
        public Color EndColor;
        public float StartScale;
        public float EndScale;
        // Use this for initialization
        public void Reset(Vector2 velocity,Vector2 position,float rotation,float angularVelocity, float holdTime,float decayTime)
        {
            transform.position = position;
            gameObject.SetActive(true);
            
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = StartColor;
            _renderer.transform.localScale = new Vector3(StartScale, StartScale, StartScale);
            
            rigidbody2D.rotation = rotation;
            rigidbody2D.position = position;
            
            rigidbody2D.velocity = velocity;
            
            rigidbody2D.angularVelocity = angularVelocity;                        

            _decayRate = 1f/ decayTime;
            _health = 1f;            
            InvokeRepeating("Decay",holdTime,Time.fixedDeltaTime);         
        }

        public void Decay()
        {
            
            _health -= _decayRate*Time.fixedDeltaTime;
            if (_health > 0)
            {
                
                lock (this)
                {
                    var position = rigidbody2D.position;
                    var scale = Mathf.Lerp(EndScale, StartScale, _health);
                    _renderer.transform.localScale = new Vector3(scale, scale, scale);
                    rigidbody2D.position = position;
                }
                
                var nextColor = Color.Lerp(EndColor, StartColor, _health);
                _renderer.color = nextColor;
            }
            else
            {
                _renderer.color = Color.clear;
                CancelInvoke("Decay");
                gameObject.SetActive(false);                   
            }  
        }
        
        public bool StillAlive { get { return _health > 0; } }
        
	
    }
}
