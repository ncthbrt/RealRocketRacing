using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace RealRocketRacing.StartMenu{
public class StartMenuScript : MonoBehaviour {
	

		public void StartGame(){
				Application.LoadLevel ("Industry");
		}

		public void Credits(){
			Application.LoadLevel ("Credits");
		}

		public void Controls(){
			Application.LoadLevel ("Controls");
		}
		public void Exit(){
			Application.Quit ();
		}

}
}