using UnityEngine;
using System.Collections;

namespace RealRocketRacing.Rocket
{

    public class RocketScreenWrap : MonoBehaviour
    {        
        private SpriteRenderer[] _renderers;        
        // Use this for initialization
        private void Start()
        {

            _renderers = new SpriteRenderer[]{ GetComponent<SpriteRenderer>()};
            
        }


        private bool CheckRenderers()
        {
            foreach (var renderer in _renderers)
            {
                // If at least one render is visible, return true
                if (renderer.isVisible)
                {
                    return true;
                }
            }

            // Otherwise, the object is invisible
            return false;
        }

        private bool _isWrappingX = false;
        private bool _isWrappingY = false;

        private void ScreenWrap()
        {
            var isVisible = CheckRenderers();

            if (isVisible)
            {                
                _isWrappingX = false;
                _isWrappingY = false;
                return;
            }            

            if (_isWrappingX && _isWrappingY)
            {
                return;
            }

            var cam = Camera.main;
            var viewportPosition = cam.WorldToViewportPoint(transform.position);
            var newPosition = transform.position;

            if (!_isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
            {
                newPosition.x = -newPosition.x;

                _isWrappingX = true;
            }

            if (!_isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
            {
                newPosition.y = -newPosition.y;

                _isWrappingY = true;
            }

            transform.position = newPosition;
        }

        // Update is called once per frame
        private void Update()
        {            
            ScreenWrap();
        }

    }
}