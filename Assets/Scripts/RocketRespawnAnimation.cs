using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class RocketRespawnAnimation : MonoBehaviour
{
    public PhysicalParticle2D Template;
    public Rigidbody2D RocketRigidbody2D;
    public RocketDamageSystem DamageSystem;
    public RocketRaceMetrics Metrics;
    
    public RocketController Controller;
    public float AdditionalVelocity;
    private IList<PhysicalParticle2D> _particles;
    

    public Transform OffscreenGarage;
    public int ParticleCount;
    public float DecayTime;    
    private const float Offset = 0.2f;
    

	void Start ()
	{
	    DecayTime= Metrics.RespawnTime;

        _particles=new PhysicalParticle2D[ParticleCount];
        for (int i = 0; i < _particles.Count; ++i)
        {
            _particles[i] = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;
        }

        DamageSystem.AddRespawnCallback(Respawn);    
	}

    void Respawn(GameObject rocket,Collision2D collision)
    {
        Debug.Log("Respawn");
        
        var rocketRigidbody = rocket.rigidbody2D;
        var location = Vector2.Lerp(collision.contacts[0].point,rocketRigidbody.position,0.5f);
        var rocketVelocity = rocketRigidbody.velocity;
        Controller.ControlsEnabled = false;
        
        rocketRigidbody.velocity = Vector2.zero;
        rocketRigidbody.angularVelocity = 0;
        rocketRigidbody.rotation = 0;
        rocketRigidbody.position = OffscreenGarage.position;
        
        foreach (var particle in _particles)
        {
         
            var velocity = rocketVelocity+ AdditionalVelocity*Random.insideUnitCircle;
            particle.Reset(velocity,location+Random.insideUnitCircle*Random.value*Offset,180*Random.value,Random.value*20f-10f,Metrics.RespawnTime-DecayTime,DecayTime);
        }

        
        Invoke("RestorePosition",Metrics.RespawnTime);
    }


    void RestorePosition()
    {
        Controller.ControlsEnabled = true;
        Metrics.ToCurrentCheckpoint();
        DamageSystem.RespawnComplete();
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
