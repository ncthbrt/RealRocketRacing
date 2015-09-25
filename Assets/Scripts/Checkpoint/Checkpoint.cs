using UnityEngine;

namespace RealRocketRacing.RaceCheckpoints
{
    public class Checkpoint : MonoBehaviour
    {
        public int CheckpointID;       
    
		public Transform TopVertex;
		public Transform BottomVertex;
        public Transform SpawnPoint;
        private SpawnPoint _spawnPoint;

        public void Start()
        {            
            SpawnPoint.transform.position = (TopVertex.position + BottomVertex.position) / 2f;
            _spawnPoint = SpawnPoint.GetComponent<SpawnPoint>();
        }
        

		public Vector2 Top{
			get{
				return TopVertex.position;
			}
		}

		public Vector2 Bottom{
			get{
				return BottomVertex.position;
			}
		}

        public bool CanSpawn
        {
            get { return _spawnPoint.CanSpawn; }
        }

        public Vector2 Location
        {
            get { return (Top+Bottom)/2f; }
        }

        public float Heading
        {
			get{
				return transform.rotation.eulerAngles.z-90f;
			}            
        }



    

    }
}
