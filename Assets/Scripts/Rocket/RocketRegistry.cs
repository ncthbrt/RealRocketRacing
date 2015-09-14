using UnityEngine;
using System.Collections;
namespace RealRocketRacing.Rocket{
	public class RocketRegistry : MonoBehaviour {

		public GameObject[] RocketTemplates;
		public GameObject[] Rockets{ get; set;}
		public void Init(int playerCount){
			Rockets=new GameObject[playerCount];
			for (int i=0; i<playerCount; ++i) {
				Rockets[i]=RocketTemplates[i];
			}
			for (int i=playerCount; i<RocketTemplates.Length; ++i) {
				Destroy(RocketTemplates[i]);
			}
		}

	}
}