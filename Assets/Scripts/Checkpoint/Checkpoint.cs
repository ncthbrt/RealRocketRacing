using UnityEngine;

namespace RealRocketRacing.RaceCheckpoints
{
    public class Checkpoint : MonoBehaviour
    {
        public int CheckpointID;       
    
		public Transform TopVertex;
		public Transform BottomVertex;

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


        public Vector2 Location
        {
            get { return Vector2.Lerp(Top,Bottom,0.5f); }
        }

        public float Heading
        {
			get{
				return transform.rotation.eulerAngles.z-90f;
			}            
        }



    

    }
}
