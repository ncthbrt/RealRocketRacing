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
            GetComponent<SpriteRenderer>().color = BaseColor;
        }
    }
}