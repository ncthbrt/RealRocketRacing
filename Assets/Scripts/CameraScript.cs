using UnityEngine;

namespace Assets.Scripts
{
    public class CameraScript : MonoBehaviour
    {

        public Rigidbody2D Rocket;
        private float _prev; 
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
        }



        void FixedUpdate ()
        {
            Debug.Log("Framerate: " + 1f / Time.unscaledDeltaTime);
            Vector2 delta = Rocket.position - new Vector2(transform.position.x, transform.position.y);            
            transform.Translate(delta);
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
    }
}
