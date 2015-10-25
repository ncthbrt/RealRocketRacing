using UnityEngine;
using System.Collections;
using RealRocketRacing.Rocket;

namespace RealRocketRacing{
	public class PivotSplitScreen : MonoBehaviour {

		public RocketRegistry Registry;
		public Camera MainCamera;
		private Rigidbody2D _rocket1;
	    public MeshFilter Rocket1CameraSurface;
        public MeshFilter Rocket2CameraSurface;
        private Rigidbody2D _rocket2;
	    private float _circumscribe_angle;
		// Use this for initialization
		void Start () {
			_rocket1 = Registry.Rockets [0].GetComponent<Rigidbody2D> ();
			_rocket2 = Registry.Rockets [1].GetComponent<Rigidbody2D> ();
            Rocket1CameraSurface.mesh.Clear();
            Rocket1CameraSurface.mesh.triangles=new int[4] {0,1,2,3};
            Rocket2CameraSurface.mesh.triangles = new int[4] { 0, 1, 2, 3 };
            Rocket1CameraSurface.mesh.vertices= new Vector3[4];
            Rocket1CameraSurface.mesh.uv = new Vector2[4];
            _circumscribe_angle = Mathf.Asin(1.0f/MainCamera.aspect);
		}



		// Update is called once per frame
	    private void Update()
	    {
	        var midpoint = (_rocket1.position + _rocket2.position)/2.0f;
	        var d1 = _rocket1.position - midpoint;
	        var angle = Mathf.Atan2(d1.y, d1.x);
	        if (angle < 0)
	        {
	            angle = 2*Mathf.PI - angle;
	        }

	        var lineAngle = angle + Mathf.PI/2.0f;
	        lineAngle %= (2*Mathf.PI);

	        Vector3[] rocket1Shape = new Vector3[4];
	        Vector3[] rocket2Shape = new Vector3[4];

	        if (lineAngle < _circumscribe_angle || lineAngle >= 2*Mathf.PI - _circumscribe_angle)
	        {
	            var hyp = 1.0f/Mathf.Cos(lineAngle);
	            var y = (Mathf.Sqrt(hyp*hyp - 1f) + 1f)/2f;
	            var p1 = new Vector3(1.0f, y);
	            var p2 = new Vector3(0, 1.0f - y);

	            Vector3[] a = new Vector3[4] {p1, p2, new Vector3(0, 0), new Vector3(1, 0)};
	            Vector3[] b = new Vector3[4] {p1, new Vector3(1, 1), new Vector3(0, 1), p2};
	            if (lineAngle <= _circumscribe_angle)
	            {
	                rocket1Shape = a;
	                rocket2Shape = b;
	            }
	            else
	            {
	                rocket1Shape = b;
	                rocket2Shape = a;
	            }
	        }
	        else if (lineAngle < Mathf.PI - _circumscribe_angle)
	        {
	            var hyp = 1/Mathf.Sin(lineAngle);
	            var x = (Mathf.Sqrt(hyp*hyp - 1.0f) + 1f)/2f;
	            var p1 = new Vector3(x, 1.0f);
	            var p2 = new Vector3(1 - p1.x, 0.0f);
	            Vector3[] a = new Vector3[4] {p1, p2, new Vector3(1, 0), new Vector3(1, 1)};
	            Vector3[] b = new Vector3[4] {p1, new Vector3(0, 1), new Vector3(0, 0), p2};

	            if (lineAngle <= Mathf.PI/2f)
	            {
	                rocket1Shape = a;
	                rocket2Shape = b;
	            }
	            else
	            {
	                rocket1Shape = b;
	                rocket2Shape = a;
	            }
	        }
	        else if (lineAngle < Mathf.PI + _circumscribe_angle)
	        {
	            var altLineAngle = Mathf.PI*2 - lineAngle;
	            var hyp = 1.0f/Mathf.Cos(altLineAngle);
	            var y = (Mathf.Sqrt(hyp*hyp - 1f) + 1f)/2f;
	            var p1 = new Vector3(1.0f, y);
	            var p2 = new Vector3(0, 1.0f - y);

	            Vector3[] a = new Vector3[4] {p1, p2, new Vector3(0, 0), new Vector3(1, 0)};
	            Vector3[] b = new Vector3[4] {p1, new Vector3(1, 1), new Vector3(0, 1), p2};

	            if (lineAngle <= Mathf.PI)
	            {
	                rocket2Shape = a;
	                rocket1Shape = b;
	            }
	            else
	            {
	                rocket2Shape = b;
	                rocket1Shape = a;
	            }
	        }
	        else if (lineAngle < 2*Mathf.PI - _circumscribe_angle)
	        {
	            var altLineAngle = Mathf.PI*2 - lineAngle;
	            var hyp = 1/Mathf.Sin(lineAngle);
	            var x = (Mathf.Sqrt(hyp*hyp - 1.0f) + 1f)/2f;
	            var p1 = new Vector3(x, 1.0f);
	            var p2 = new Vector3(1 - p1.x, 0.0f);
	            Vector3[] a = new Vector3[4] {p1, p2, new Vector2(1, 0), new Vector3(1, 1)};
	            Vector3[] b = new Vector3[4] {p1, new Vector3(0, 1), new Vector3(0, 0), p2};

	            if (lineAngle <= Mathf.PI/2f)
	            {
	                rocket2Shape = a;
	                rocket1Shape = b;
	            }
	            else
	            {
	                rocket2Shape = b;
	                rocket1Shape = a;
	            }
	        }
            Rocket1CameraSurface.mesh.Clear(true);
            Rocket2CameraSurface.mesh.Clear(true);


            Rocket1CameraSurface.mesh.vertices = rocket1Shape;            
            Rocket2CameraSurface.mesh.vertices = rocket2Shape;

            Rocket1CameraSurface.mesh.uv = new Vector2[4] { rocket1Shape[0], rocket1Shape[1], rocket1Shape[2], rocket1Shape[3] };
            Rocket2CameraSurface.mesh.uv= new Vector2[4] {rocket2Shape[0], rocket2Shape[1] , rocket2Shape[2] , rocket2Shape[3] };
            

        }
	}
}