using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class RocketRespawnAnimation : MonoBehaviour
{
    public PhysicalParticle2D Template;
    public Rigidbody2D RocketRigidbody2D;
    public RocketDamageSystem DamageSystem;
    
    private IList<PhysicalParticle2D> _particles;
    public float SpawnRate;
	// Use this for initialization
	void Start () {
	    DamageSystem.AddRespawnCallback(Respawn);    
        _particles=new List<PhysicalParticle2D>();
	}

    void Respawn(GameObject rocket,Collision2D collision)
    {
        var normal = collision.contacts[0].normal;
        var angle=(RocketRigidbody2D.rotation - 90)/180*Mathf.PI;
        PolygonCollider2D polygonCollider=rocket.GetComponent<PolygonCollider2D>();

        var bounds=polygonCollider.bounds;
        var width=bounds.size.x;
        var height=bounds.size.y;
        var center = bounds.center;
        var bottomLeft = (Vector2)center - new Vector2(width / 2, height / 2);
        

        for (int j = 0; j < height/Template.renderer.bounds.size.y; j++)
        {
            for (int i = 0; i < width/Template.renderer.bounds.size.x; i++)
            {
                var particle = Instantiate(Template, Vector3.zero, Quaternion.identity) as PhysicalParticle2D;
                var position = bottomLeft + new Vector2(Mathf.Cos(angle)*i*Template.renderer.bounds.size.x,Mathf.Sin(angle)*j*Template.renderer.bounds.size.y);
                _particles.Add(particle);
                particle.Reset(RocketRigidbody2D.velocity,position,0,0,0,1f);
            }
        }
        rigidbody2D.velocity=(Vector2.zero);
     //   rigidbody2D.position=new Vector2(-200,82);
        rigidbody2D.angularVelocity = 0;

        gameObject.SetActive(false);

    }


    void RestorePosition()
    {
        
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
