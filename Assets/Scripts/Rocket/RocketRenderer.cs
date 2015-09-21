using UnityEngine;

namespace RealRocketRacing.Rocket
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
            _renderer=GetComponent<SpriteRenderer>();
            var pallete = GetComponent<RocketPalette>();
            if (pallete == null)
            {
                pallete = GetComponentInParent<RocketPalette>();
            }
            _damageSystem = GetComponent<RocketDamageSystem>();
            
            _baseColor = pallete.BaseColor;
            _damageColor = pallete.AccentColor;
            _currentColor = _baseColor;
            _damageSystem.AddOnDamageCallback(RocketDamage);            
            _damageSystem.AddOnDamageCallback(RocketDamage);
          //  _damageSystem.AddRespawnCallback(Respawn);
        }

        private void RocketDamage(GameObject rocket, float damage, float remainingHealth)
        {

            CancelInvoke("ToWhite");            
            _exitDelta = Time.fixedDeltaTime / DamageColorExit;
            _progress = 0;            
            _currentColor = _damageColor;                            
            _renderer.color = _currentColor;
            InvokeRepeating("ToWhite", DamageColorHold, Time.fixedDeltaTime);
        }

        public Color _damageColor=new Color(1,0,0);                
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

        
    }
}
