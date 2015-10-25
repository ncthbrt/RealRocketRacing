using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;

namespace RealRocketRacing{
	public class PivotSplitScreen : MonoBehaviour {

		public RocketRegistry Registry;
		public Camera MainCamera;
		private Rigidbody2D _rocket1;
		private Rigidbody2D _rocket2;
		// Use this for initialization
		void Start () {
			_rocket1 = Registry.Rockets [0].GetComponent<Rigidbody2D> ();
			_rocket2 = Registry.Rockets [1].GetComponent<Rigidbody2D> ();
		}



		// Update is called once per frame
		void Update () {
		
			float y2 = _rocket2.position.y;
			float y1 = _rocket1.position.y;
			float x2 = _rocket2.position.x;
			float x1 = _rocket1.position.x;

			float m = (y2 - y1) / (x2 - x1);

			Vector2 top, bottom;
			if (m > 1) {
				top = new Vector2 (1 / m, 1);
				bottom = new Vector2 (-1 / m, -1);
			} else if (m > 0) {
				top = new Vector2 (1, m);
				bottom = new Vector2 (0, -m);
			} else if (m < -1) {
				top = new Vector2 (1, m);
				bottom = new Vector2 (-1, -m);
			} else {
				top = new Vector2 (1, m);
				bottom = new Vector2(-1, -m);
			}
//			
//			float y = m * (minX - x1) + y1;
//			if (y > minY && y < maxY) return true;
//			
//			y = m * (maxX - x1) + y1;
//			if (y > minY && y < maxY) return true;
//			
//			float x = (minY - y1) / m + x1;
//			if (x > minX && x < maxX) return true;
//			
//			x = (maxY - y1) / m + x1;
//			if (x > minX && x < maxX) return true;
//			
//			return false;
		}
	}
}