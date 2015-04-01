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
            _baseColor = _renderer.color;
            _damageSystem.AddOnDamageCallback(RocketDamage);            
            _damageSystem.AddOnDamageCallback(RocketDamage);
          //  _damageSystem.AddRespawnCallback(Respawn);
        }

        private const float CriticalHealth = 0.2f;
        private const float ModerateHealth= 0.5f;        
        private void RocketDamage(GameObject rocket, float damage, float remainingHealth)
        {

            CancelInvoke("ToWhite");            
            _exitDelta = Time.fixedDeltaTime / DamageColorExit;
            _progress = 0;            
            _currentColor = DamageColor;                            
            _renderer.color = _currentColor;
            InvokeRepeating("ToWhite", DamageColorHold, Time.fixedDeltaTime);
        }

        private Color DamageColor=new Color(1,0,0);                
        private Color _currentColor;
        private Color _baseColor;
        private float _progress=1f;
        private float _exitDelta;

        private void ToWhite()
        {
            _progress += _exitDelta;
            _renderer.color = Color.Lerp(_currentColor,_baseColor,_progress);
            if (_progress >= 1)
            {
                CancelInvoke("ToWhite");
            }
        }

        






        // Update is called once per frame
        void Update () {
	
        }
    }
}
