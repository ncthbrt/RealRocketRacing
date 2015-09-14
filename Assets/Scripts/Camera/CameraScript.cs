using UnityEngine;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.RRRCamera
{
    public class CameraScript : MonoBehaviour
    {
		public RocketRegistry Registry;
        
		private Rigidbody2D[] _players;	
		float BoundingMultiplier=1.4f;
        private float _prev;
        private RocketDamageSystem[] DamageSystems;
		private RocketRaceMetrics[] Metrics;
		public float CollisionShakeTime=0.3f;
        
        public float ExponentialEasingFactor=0.25f;
        public float StartSize = 8;    
        public float MaxSize = 20;
        public float MaxSpeed;
        private float _invExpFactor;
        private float _sizeDelta;
		private float _aspect;

        private void Start()
        {
            _prev = 0;
			_aspect = GetComponent<Camera>().aspect;
			_players = new Rigidbody2D[Registry.Rockets.Length];

			DamageSystems=new RocketDamageSystem[_players.Length];
			Metrics = new RocketRaceMetrics[_players.Length];
			for (int i=0; i<_players.Length; ++i) {
				_players[i]=Registry.Rockets[i].GetComponent<Rigidbody2D>();
				Metrics[i]=_players[i].GetComponent<RocketRaceMetrics>();
				DamageSystems[i]=_players[i].GetComponent<RocketDamageSystem>();
				DamageSystems[i].AddRespawnCallback(DestructionShake);
				DamageSystems[i].AddOnDamageCallback (DamageShake);
			}

            _invExpFactor = 1 - ExponentialEasingFactor;
            _sizeDelta = MaxSize - StartSize;
		}



        void FixedUpdate ()
        {

			Vector2 averagePosition=new Vector2();
			float maxSpeed = 0;
			Vector2 max = new Vector2 ();
			Vector2 min = new Vector2 (float.PositiveInfinity, float.PositiveInfinity);

			int aliveCount = 0;
			for (int i=0; i<_players.Length; ++i){
				if(DamageSystems[i].Alive){

					aliveCount++;
					var mag=_players[i].velocity.magnitude;
					maxSpeed=maxSpeed<mag?mag:maxSpeed;
					averagePosition+=_players[i].position;
					var pos=_players[i].position;
					if(pos.x>max.x){
						max.x=pos.x;
					}
					if(pos.y>max.y){
						max.y=pos.y;
					}

					if(pos.x<min.x){
						min.x=pos.x;
					}
					if(pos.y<min.y){
						min.y=pos.y;
					}
				}
			}



            if (aliveCount>0)
            {

				Vector2 deltaExtremes = max - min;
				float reqH = deltaExtremes.x / _aspect;
				reqH = reqH < deltaExtremes.y ? deltaExtremes.x : reqH;


				averagePosition /= aliveCount;
				Vector2 shakeOffset=Vector2.zero;
				if(_shakeIntensity>0){
					shakeOffset = Random.insideUnitSphere * _shakeIntensity;
					transform.rotation= Quaternion.Euler(0, 0, _originalRotation + Random.Range(-_shakeIntensity, _shakeIntensity)*.2f);                    
					_shakeIntensity -= _shakeDecay;
				}


                
                Vector2 delta = averagePosition - new Vector2(transform.position.x, transform.position.y);
                transform.Translate(delta+shakeOffset);



            	_prev = maxSpeed*ExponentialEasingFactor + _invExpFactor*_prev;

				float finalSize;
                if (_prev > 0)
                {
                    if (_prev < MaxSpeed)
                    {
                        finalSize = _sizeDelta*(_prev/MaxSpeed) + StartSize;
                    }
                    else
                    {

                        finalSize = MaxSize;
                    }
                }
                else
                {
                    finalSize = StartSize;
                }
				finalSize=Mathf.Max(reqH*BoundingMultiplier,finalSize);
				GetComponent<Camera>().orthographicSize=finalSize;
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
			_originalRotation = transform.rotation.eulerAngles.z;
		}

		public void DestructionShake(GameObject rocket,Vector2 Other){
			StartLargeShake();
		}
        public void StartLargeShake()
        {
            _shakeIntensity = 0.6f;
            _shakeDecay= _shakeIntensity*Time.fixedDeltaTime/Metrics[0].RespawnTime;
            _originalLocation = transform.position;
            _originalRotation = transform.rotation.eulerAngles.z;
        }
    }
}
