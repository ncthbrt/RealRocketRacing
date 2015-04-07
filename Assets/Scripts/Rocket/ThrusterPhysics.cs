using UnityEngine;

namespace RealRocketRacing.Rocket
{
    public class ThrusterPhysics : MonoBehaviour
    {


        public float ImpulsePerFixedUpdate;    
        // Use this for initialization
        void Start () {
	
        }

        public void ApplyThrust(Rigidbody2D rigidbody2D)
        {
            var postion = new Vector2(transform.position.x, transform.position.y);
            float angle = ((transform.eulerAngles.z)/180f)*Mathf.PI-Mathf.PI;
            var thrust = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * ImpulsePerFixedUpdate;
            rigidbody2D.AddForceAtPosition(thrust, postion, ForceMode2D.Impulse);
        }
    }
}
