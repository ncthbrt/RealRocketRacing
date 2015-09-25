using UnityEngine;
using System.Collections;

namespace RealRocketRacing.RaceCheckpoints
{
    public class SpawnPoint : MonoBehaviour
    {
        private bool _canSpawn = true;

        public bool CanSpawn
        {
            get { return _canSpawn; }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _canSpawn = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _canSpawn = true;
        }
    }
}