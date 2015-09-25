using UnityEngine;
using System.Collections;
using RealRocketRacing.ParticleSystem;
using System.Collections.Generic;

namespace RealRocketRacing.Rocket{

	public class RocketRespawnAnimation : MonoBehaviour
	{
	    public PhysicalParticle2D Template;
	    private Rigidbody2D _rocketRigidbody2D;
		private ExplosionParticleSystem _explosionParticleSystem;
	    public RocketDamageSystem DamageSystem;
	    public RocketRaceMetrics Metrics;
	    public AudioSource RespawnAudio;
	    
	    public RocketController Controller;	    	   
	    

	    public Transform OffscreenGarage;	    

		void Start ()
		{
		    _rocketRigidbody2D = GetComponent<Rigidbody2D>();
			_explosionParticleSystem = GetComponent<ExplosionParticleSystem> ();
			_explosionParticleSystem.MaxParticleDecayTime= Metrics.RespawnTime;
			_explosionParticleSystem.MinParticleDecayTime= Metrics.RespawnTime*0.5f;
			_explosionParticleSystem.Setup ();
	        DamageSystem.AddRespawnCallback(Respawn);    
		}

	    void Respawn(GameObject rocket,Vector2 point)
	    {
	              
	        var rocketRigidbody = rocket.GetComponent<Rigidbody2D>();
	        var location = Vector2.Lerp(point,rocketRigidbody.position,0.5f);
	        var rocketVelocity = rocketRigidbody.velocity;
	        Controller.ControlsEnabled = false;
	        
	        rocketRigidbody.velocity = Vector2.zero;
	        rocketRigidbody.angularVelocity= 0;            
	        rocketRigidbody.rotation = 0;
	        rocketRigidbody.position = OffscreenGarage.position;
	        
			_explosionParticleSystem.StartExplosion (rocketVelocity, location);	        
	        
	        InvokeRepeating("RestorePosition",Metrics.RespawnTime,1/60f);
	    }


	    void RestorePosition()
	    {
	        if (Metrics.CurrentCheckpoint.CanSpawn)
	        {
	            Metrics.ToCurrentCheckpoint();
                RespawnAudio.Play();
	            DamageSystem.RespawnComplete();
	            Controller.ControlsEnabled = true;
                CancelInvoke("RestorePosition");
	        }	        
	    }
	
	}
}