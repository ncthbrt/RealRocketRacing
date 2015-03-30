using UnityEngine;

namespace Assets.Scripts
{
    public class RocketRenderer : MonoBehaviour
    {
        private RocketDamageSystem _damageSystem;
        private SpriteRenderer _renderer;

        public float DamageColorHold = 1f;
        public float DamageColorExit = 0.3f;
        // Use this for initialization
        void Start ()
        {
            _damageSystem = GetComponent<RocketDamageSystem>();
            _renderer = GetComponent<SpriteRenderer>();
            _damageSystem.AddOnDamageCallback(RocketDamage);            
            _damageSystem.AddOnDamageCallback(RocketDamage);
            _damageSystem.AddRespawnCallback(Respawn);
        }

        private const float CriticalHealth = 0.2f;
        private const float ModerateHealth= 0.5f;        
        private void RocketDamage(GameObject rocket, float damage, float remainingHealth)
        {
            CancelInvoke("ToWhite");            
            _exitDelta = Time.fixedDeltaTime / DamageColorExit;
            _progress = 0;            
            _currentColor = _red;                            
            _renderer.color = _currentColor;
            InvokeRepeating("ToWhite", DamageColorHold, Time.fixedDeltaTime);
        }

        private Color _red=new Color(1,0,0);        
        private Color _orange=new Color(1f,0.49f,0f);
        private Color _black=new Color(0,0,0);
        private Color _currentColor;

        private float _progress=1f;
        private float _exitDelta;

        private void ToWhite()
        {
            _progress += _exitDelta;
            _renderer.color = new Color(_currentColor.r + (1 - _currentColor.r) * _progress, _currentColor.g + (1 - _currentColor.g) * _progress, _currentColor.b + (1 - _currentColor.b) * _progress);
            if (_progress >= 1)
            {
                CancelInvoke("ToWhite");
            }
        }

        

        private void Respawn(GameObject rocket)
        {
            
        }




        // Update is called once per frame
        void Update () {
	
        }
    }
}
