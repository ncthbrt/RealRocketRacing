using UnityEngine;
using System.Collections;

namespace RealRocketRacing.Rocket
{
    public class RocketPalette : MonoBehaviour
    {
        public Color BaseColor;
        public Color AccentColor;

        public void Start()
        {            
            ResetColors();
        }

        public void ResetColors()
        {
            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().color = BaseColor;
            }
        }
    }
}