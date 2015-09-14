using UnityEngine;
using RealRocketRacing.RaceCheckpoints;
namespace RealRocketRacing.Rocket
{
    public class RocketDamageSystem : MonoBehaviour {
	
        void Start () {		
            Health = 1f;
            Alive =true;
            _metrics = GetComponent<RocketRaceMetrics>();
			_metrics.AddPassedInvalidCheckpointCallback (PassedInvalidCheckpoint);
        }

		private void PassedInvalidCheckpoint(Checkpoint checkpoint,GameObject rocket){
			Vector2 position = rocket.transform.position;
			var top = checkpoint.Top;
			Vector2 bottom = checkpoint.Bottom;
			Vector2 normal = (top-bottom);
			Vector2 delta = position - bottom;
			normal.Normalize ();
			var bCosTheta = Vector2.Dot (normal, delta);
			normal *= bCosTheta;
			InduceRespawn (normal+bottom);
		}

        private RocketRaceMetrics _metrics;

        public float DamageScalingFactor;//Determines how much damage is receive



        public float Health { get; set; }
        

        public delegate void RocketDamage(GameObject rocket, float damage,float remainingHealth);    

        public delegate void Respawn(GameObject rocket,Vector2 contactPoint);



		public void InduceRespawn (Vector2 location){
			Alive = false;
			
			if (_respawnCallbacks != null)
			{                        
				_respawnCallbacks(gameObject,location);
			}           	            
		}
		
		
		private Respawn _respawnCallbacks;    
		private RocketDamage _damagedCallbacks;    
   
    
	


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
			Health = 1f;
        }

    


        public void OnCollisionEnter2D(Collision2D other)
        {
			if(!other.gameObject.tag.Equals("Effects")){
				if(other.gameObject.tag.Equals("Fatal")){
					InduceRespawn(other.contacts[0].point);
				}

	            var relativeSpeed = other.relativeVelocity.magnitude;
	            var otherNormal = other.contacts[0].normal;
	            var dirNorm= GetComponent<Rigidbody2D>().velocity.normalized;
	            var angleScaleFactor=Mathf.Abs(Vector2.Dot(otherNormal,dirNorm));                        
	            var damage = relativeSpeed*angleScaleFactor*DamageScalingFactor;

	            Health -= damage;
	            Health = Health < 0 ? 0 : Health;//Ensure health never below 0

	            if (Health <=0)
	            {
					InduceRespawn(other.contacts[0].point);
	            }
	            else if (_damagedCallbacks != null)
                {
	                    _damagedCallbacks(gameObject, damage, Health);
                }
			}
        }
	 
        
	
	

	
    }
}
