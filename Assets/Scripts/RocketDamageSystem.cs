using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class RocketDamageSystem : MonoBehaviour {
	
	void Start () {		
	    LivesRemaining = LivesAtStart;
	    Health = 1f;
	    _respawnCallbacks += ResetPosition;
	    _metrics = GetComponent<RocketRaceMetrics>();
	}

    private int damageCount = 0;

    private RocketRaceMetrics _metrics;    

    public float DamageScalingFactor=10;//Determines how much damage is receive


    void Update() { }

    public int LivesAtStart= 3;
    public float Health { get; set; }
    public int LivesRemaining { get; private set; }

    public delegate void RocketDamaged(GameObject rocket, float damage,float remainingHealth);
    public delegate  void RocketCritical(GameObject rocket,float damage,float remainingHealth);//When the rocket has less than 10% health remaining
    public delegate void Respawn(GameObject rocket);

    public delegate void NoLivesRemaining(GameObject rocket);

    private Respawn _respawnCallbacks;
    private RocketCritical _criticalCallbacks;
    private RocketDamaged _damagedCallbacks;
    private NoLivesRemaining _noLivesRemainingCallbacks;

	public float DamageVelocityThreshold;//How fast do you need to go before damage is received


    public void AddRespawnCallback(Respawn callback)
    {
        _respawnCallbacks += callback;
    }

    public void AddOnDamageCallback(RocketDamaged callback)
    {
        _damagedCallbacks+= callback;
    }
    public void AddRocketCriticalCallback(RocketCritical callback)
    {
        _criticalCallbacks+= callback;
    }

    public void AddNoLivesRemainingCallback(NoLivesRemaining callback)
    {
        _noLivesRemainingCallbacks += callback;
    }

	public void OnCollisionEnter2D(Collision2D other)
	{
	    var relativeSpeed = other.relativeVelocity.magnitude;
	    if ( relativeSpeed> DamageVelocityThreshold)//Damage will be received
	    {
	        var damage = (relativeSpeed-DamageVelocityThreshold)/DamageScalingFactor;
	        Health -= damage;
	        if (Health <=0)
	        {
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
	                    _respawnCallbacks(gameObject);
	                }
	                Health = 1;
	            }	            	            
	        }
            else if (Health < 0.2f)
            {
                if (_criticalCallbacks != null)
                {
                    _criticalCallbacks(gameObject, damage, Health);
                }
            }
            else
            {
                Debug.Log("Damage #"+damageCount+++": "+damage+", Health:"+Health);
                if (_damagedCallbacks != null)
                {
                    _damagedCallbacks(gameObject, damage, Health);
                }
            }
	    }
	}
	
	private void ResetPosition(GameObject rocket){
		var checkpoint = _metrics.CurrentCheckpoint;		
        Debug.Log("Respawning");
		rigidbody2D.position=checkpoint.Location;
		rigidbody2D.rotation=(checkpoint.Heading);
        rigidbody2D.angularVelocity = 0;
        rigidbody2D.velocity=new Vector2(0,0);	    
	}

	
}
