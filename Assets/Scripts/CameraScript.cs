using UnityEngine;

namespace Assets.Scripts
{
    public class CameraScript : MonoBehaviour
    {

        public Rigidbody2D Rocket;
        private float _prev;
        public RocketDamageSystem DamageSystem;
		public float CollisionShakeTime=0.3f;
        public RocketRaceMetrics Metrics;
        public float ExponentialEasingFactor=0.25f;
        public float StartSize = 8;    
        public float MaxSize = 20;
        public float MaxSpeed;
        private float _invExpFactor;
        private float _sizeDelta;
        private void Start()
        {
            _prev = 0;
            _invExpFactor = 1 - ExponentialEasingFactor;
            _sizeDelta = MaxSize - StartSize;
            DamageSystem.AddRespawnCallback(DestructionShake);
			DamageSystem.AddOnDamageCallback (DamageShake);
        }



        void FixedUpdate ()
        {
            if (DamageSystem.Alive)
            {

				Vector2 shakeOffset=Vector2.zero;
				if(_shakeIntensity>0){
					shakeOffset = Random.insideUnitSphere * _shakeIntensity;
					transform.rotation= Quaternion.Euler(0, 0, _originalRotation + Random.Range(-_shakeIntensity, _shakeIntensity)*.2f);                    
					_shakeIntensity -= _shakeDecay;
				}


                //Debug.Log("Framerate: " + 1f/Time.unscaledDeltaTime);
                Vector2 delta = Rocket.position - new Vector2(transform.position.x, transform.position.y);
                transform.Translate(delta+shakeOffset);

                var speed = Rocket.velocity.magnitude;

                _prev = speed*ExponentialEasingFactor + _invExpFactor*_prev;


                if (_prev > 0)
                {
                    if (_prev < MaxSpeed)
                    {
                        camera.orthographicSize = _sizeDelta*(_prev/MaxSpeed) + StartSize;
                    }
                    else
                    {
                        camera.orthographicSize = MaxSize;
                    }
                }
                else
                {
                    camera.orthographicSize = StartSize;
                }
            }
            else
            {
                if (_shakeIntensity > 0)
                {
                    transform.position = _originalLocation+ Random.insideUnitSphere * _shakeIntensity;
                    transform.rotation= Quaternion.Euler(0, 0, _originalRotation + Random.Range(-_shakeIntensity, _shakeIntensity)*.2f);                    
                    _shakeIntensity -= _shakeDecay;
                }                                
            }
        }

        private Vector3 _originalLocation;
        private float _originalRotation;
        private float _shakeDecay;
        
        private float _shakeIntensity;


		private void DamageShake(GameObject rocket, float damage,float remainingHealth){
			StartShake (0.3f,CollisionShakeTime);
		}


		public void StartShake(float intensity,float time){
			_shakeIntensity = intensity;
			_shakeDecay = _shakeIntensity * Time.fixedDeltaTime / time;
			_originalRotation = transform.rotation.z;
		}

		public void DestructionShake(GameObject rocket,Collision2D other){
			StartLargeShake();
		}
        public void StartLargeShake()
        {
            _shakeIntensity = 0.6f;
            _shakeDecay= _shakeIntensity*Time.fixedDeltaTime/Metrics.RespawnTime;
            _originalLocation = transform.position;
            _originalRotation = transform.rotation.z;
        }
    }
}
