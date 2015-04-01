using UnityEngine;

namespace Assets.Scripts
{
    public class RocketDamageSystem : MonoBehaviour {
	
        void Start () {		
            LivesRemaining = LivesAtStart;
            Health = 1f;
            Alive =true;
            _metrics = GetComponent<RocketRaceMetrics>();
        }

        private int damageCount = 0;

        private RocketRaceMetrics _metrics;

        public float DamageScalingFactor;//Determines how much damage is receive


        void Update() { }

        public int LivesAtStart= 3;
        public float Health { get; set; }
        public int LivesRemaining { get; private set; }

        public delegate void RocketDamage(GameObject rocket, float damage,float remainingHealth);    

        public delegate void Respawn(GameObject rocket,Collision2D other);

    
        public delegate void NoLivesRemaining(GameObject rocket);

        private Respawn _respawnCallbacks;    
        private RocketDamage _damagedCallbacks;    
        private NoLivesRemaining _noLivesRemainingCallbacks;
    
	


        public void AddRespawnCallback(Respawn callback)
        {
            _respawnCallbacks += callback;
        }

        public void AddOnDamageCallback(RocketDamage callback)
        {
            _damagedCallbacks+= callback;
        }

        public bool Alive { get; private set; }
        public void RespawnComplete()
        {
            Alive = true;
        }


        public void AddNoLivesRemainingCallback(NoLivesRemaining callback)
        {
            _noLivesRemainingCallbacks += callback;
        }

    


        public void OnCollisionEnter2D(Collision2D other)
        {
            var relativeSpeed = other.relativeVelocity.magnitude;
            var otherNormal = other.contacts[0].normal;
            var dirNorm= rigidbody2D.velocity.normalized;
            var angleScaleFactor=Mathf.Abs(Vector2.Dot(otherNormal,dirNorm));                        
            var damage = relativeSpeed*angleScaleFactor*DamageScalingFactor;

            Health -= damage;
            Health = Health < 0 ? 0 : Health;//Ensure health never below 0

            if (Health <=0)
            {
                Alive = false;
                LivesRemaining--;
                if (LivesRemaining == 0)
                {
                    if (_noLivesRemainingCallbacks != null)
                    {
                        _noLivesRemainingCallbacks(gameObject);
                    }
                }
                else
                {                    
                    if (_respawnCallbacks != null)
                    {
                        
                        _respawnCallbacks(gameObject,other);
                    }
                    Health = 1;
                }	            	            
            }
            else
            {                
                if (_damagedCallbacks != null)
                {
                    _damagedCallbacks(gameObject, damage, Health);
                }
            }
	 
        }
	
	

	
    }
}
