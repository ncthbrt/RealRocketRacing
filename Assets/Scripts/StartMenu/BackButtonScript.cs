using UnityEngine;
using System.Collections;
namespace RealRocketRacing.StartMenu{
public class BackButtonScript : MonoBehaviour {

		public void Back(){
			Application.LoadLevel ("StartMenu");
		}

	}
}