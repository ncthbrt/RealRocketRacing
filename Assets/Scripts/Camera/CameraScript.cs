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
        public ZoomLensFadeAnimation[] ZoomLenses;
        public float ExponentialEasingFactor=0.25f;
        public float StartSize = 8;    
        public float MaxSpeedScaleSize = 20;
        public float ZoomLensMinSize= 20;
        public float ZoomLensBuildupTime=10f;
        public float ZoomLensHideTime = 3f;
        public float MaxSpeed=15;
        private float _invExpFactor;
        private float _sizeDelta;
		private float _aspect;

        private void Start()
        {
            _prev = 0;
			_aspect = GetComponent<Camera>().aspect;
			_players = new Rigidbody2D[Registry.Rockets.Length];
            _lastPosition= new Vector2[Registry.Rockets.Length];
            _lastVelocity= new float[Registry.Rockets.Length];            
            DamageSystems =new RocketDamageSystem[_players.Length];
			Metrics = new RocketRaceMetrics[_players.Length];
			for (int i=0; i<_players.Length; ++i) {
				_players[i]=Registry.Rockets[i].GetComponent<Rigidbody2D>();
				Metrics[i]=_players[i].GetComponent<RocketRaceMetrics>();
				DamageSystems[i]=_players[i].GetComponent<RocketDamageSystem>();
				DamageSystems[i].AddRespawnCallback(DestructionShake);
				DamageSystems[i].AddOnDamageCallback (DamageShake);			
			}
            foreach (var zoomLens in ZoomLenses)
            {
                zoomLens.Hide();
            }


            _invExpFactor = 1 - ExponentialEasingFactor;
            _sizeDelta = MaxSpeedScaleSize - StartSize;
		}

        private Vector2[] _lastPosition;
        private float[] _lastVelocity;
        private float _splitTime = 0;
        private bool _isSplit;
        private float _mergeTime= 0;        
        public void Update()
        {
            for (int i = 0; i < _players.Length; ++i)
            {
                if (!DamageSystems[i].Alive)
                {
                    ZoomLenses[i].Hide();
                }
                else if(_isSplit)
                {
                    ZoomLenses[i].ReentrantFadeIn();
                }
            }

            if (GetComponent<Camera>().orthographicSize >= ZoomLensMinSize)
            {
                _splitTime += Time.deltaTime;
                _mergeTime = 0;
                if (_splitTime >= this.ZoomLensBuildupTime)
                {
                    if (!_isSplit)
                    {
                        foreach (var zoomLens in ZoomLenses)
                        {
                            zoomLens.FadeIn();
                        }
                    }
                    _isSplit = true;                    
                }
            }
            else
            {
                _mergeTime += Time.deltaTime;
                _splitTime = 0;
                if (_mergeTime >= ZoomLensHideTime)
                {
                    if (_isSplit)
                    {
                        foreach (var zoomLens in ZoomLenses)
                        {
                            zoomLens.FadeOut();
                        }
                    }
                    _isSplit = false;
                }
            }
        }

        void FixedUpdate ()
        {
			Vector2 averagePosition=new Vector2();
			float maxSpeed = 0;
			Vector2 max = new Vector2 (float.NegativeInfinity,float.NegativeInfinity);
			Vector2 min = new Vector2 (float.PositiveInfinity, float.PositiveInfinity);
			
			for (int i=0; i<_players.Length; ++i){
				if(DamageSystems[i].Alive){									
				    _lastVelocity[i] = _players[i].velocity.magnitude; 
				    _lastPosition[i] = _players[i].position;
				}
                maxSpeed = maxSpeed < _lastVelocity[i] ? _lastVelocity[i] : maxSpeed;
                averagePosition += _lastPosition[i];
			    var pos = _lastPosition[i];
                if (pos.x > max.x)
                {
                    max.x = pos.x;
                }
                if (pos.y > max.y)
                {
                    max.y = pos.y;
                }

                if (pos.x < min.x)
                {
                    min.x = pos.x;
                }
                if (pos.y < min.y)
                {
                    min.y = pos.y;
                }
            }
          
			Vector2 deltaExtremes = max - min;
			float reqH = deltaExtremes.x / _aspect;
			reqH = Mathf.Max(deltaExtremes.y,reqH);


			averagePosition /= _players.Length;
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

                    finalSize = MaxSpeedScaleSize;
                }
            }
            else
            {
                finalSize = StartSize;
            }
			finalSize=Mathf.Max(reqH*BoundingMultiplier,finalSize);
			GetComponent<Camera>().orthographicSize=finalSize;


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
