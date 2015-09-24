using UnityEngine;
using RealRocketRacing.Rocket;
namespace RealRocketRacing.RaceCheckpoints
{
    public class CheckpointRenderer : MonoBehaviour
    {
		public RocketRegistry Registry;
		bool StartingCheckPoint;
        public Camera CurrentCamera;
        public Transform StartPoint;
        public Transform EndPoint;
        private Material _material;
        private const int VertexCount=50;
        public const int NumberOfPeriods = 10;
        public const float Amplitude = 0.18f;
        


		public Color NotAvailableCheckpointLeft;
		public Color NotAvailableCheckpointRight; 

		public Color CurrentCheckpointLeft;
		public Color CurrentCheckpointRight;

		public Color NextCheckpointLeft;
		public Color NextCheckpointRight;

		public float ColorTransitionTime;

	

		private Color _currentColorLeft;
		private Color _currentColorRight;

		private Color _nextColorLeft;
		private Color _nextColorRight;
		private float _transitionProgress;
		private Checkpoint _thisCheckpoint;
        // Use this for initialization
        private LineRenderer _renderer;
		private float _colorTransitionDelta;
        private void Start()
        {

            _renderer = gameObject.AddComponent<LineRenderer>();        
            _renderer.SetVertexCount(VertexCount*2+1);
            _renderer.SetWidth(0.05f,0.05f);
            _renderer.sortingLayerName = "Player";
            _renderer.sortingOrder =0;
			_transitionProgress = 0;
			_thisCheckpoint = GetComponent<Checkpoint> ();

			var metrics = Registry.Rockets [0].GetComponent<RocketRaceMetrics> ();

			if (_thisCheckpoint == metrics.CurrentCheckpoint) {
				_currentColorLeft=CurrentCheckpointLeft;
				_currentColorRight=CurrentCheckpointRight;
			} else if (_thisCheckpoint.CheckpointID == (metrics.CurrentCheckpoint.CheckpointID + 1) % metrics.NumberOfCheckpoints) {
				_currentColorLeft=NextCheckpointLeft;
				_currentColorRight=NextCheckpointRight;
			} else {
				_currentColorLeft=NotAvailableCheckpointLeft;
				_currentColorRight=NotAvailableCheckpointRight;
			}
			_nextColorLeft = _currentColorLeft;
			_nextColorRight = _currentColorRight;
			_renderer.SetColors (_currentColorLeft,_currentColorRight); 

			_colorTransitionDelta = Time.fixedDeltaTime/ColorTransitionTime;

			for (int i=0; i<Registry.Rockets.Length; ++i) {
				Registry.Rockets[i].GetComponent<RocketRaceMetrics>().AddPassedCheckpointCallback (PassedCheckpoint);
			}
			            
            _material= new Material(Shader.Find("Particles/Additive"));
            _renderer.material =_material;
            _renderer.useWorldSpace = true;
        }

		private void PassedCheckpoint(Checkpoint checkpoint, GameObject rocket)
		{
			CancelInvoke ("Transition");
			_currentColorLeft = Color.Lerp (_currentColorLeft, _nextColorLeft, _transitionProgress);
			_currentColorRight = Color.Lerp (_currentColorRight, _currentColorLeft, _transitionProgress);

			var renderer = rocket.GetComponent<RocketPalette> ();
			var rocketColor = renderer.BaseColor;
			var metrics = rocket.GetComponent<RocketRaceMetrics> ();
			_transitionProgress = 0;
			if (checkpoint.CheckpointID == _thisCheckpoint.CheckpointID){
				_nextColorLeft=rocketColor;
				_nextColorRight=rocketColor;			
			} else if (_thisCheckpoint.CheckpointID ==(checkpoint.CheckpointID + 1) % metrics.NumberOfCheckpoints) {
				_nextColorLeft=NextCheckpointLeft;
				_nextColorRight=NextCheckpointRight;
			} else {
				_nextColorLeft=NotAvailableCheckpointLeft;
				_nextColorRight=NotAvailableCheckpointRight;
			}
			InvokeRepeating ("Transition", 0, Time.fixedDeltaTime);
		}

		private void Transition(){
			_transitionProgress += _colorTransitionDelta;
			if (_transitionProgress >= 1) {
				CancelInvoke("Transition");
				_transitionProgress=1;
			}
			var colorLeft = Color.Lerp (_currentColorLeft, _nextColorLeft, _transitionProgress);
			var colorRight = Color.Lerp (_currentColorRight, _nextColorRight, _transitionProgress);
			_renderer.SetColors (colorLeft, colorRight);

		}



        private float _progress = 0;



        private bool OnScreen(Vector3 screenCoord)
        {
            return (screenCoord.x >= 0 && screenCoord.x <= 1) && (screenCoord.y >= 0 && screenCoord.y <= 1);
        }
        // Update is called once per frame
        private void Update()
        {

            var screenCoordA= CurrentCamera.WorldToViewportPoint(StartPoint.position);
            var screenCoordB= CurrentCamera.WorldToViewportPoint(EndPoint.position);
            if (OnScreen(screenCoordA) || OnScreen(screenCoordB))
            {
                _renderer = GetComponent<LineRenderer>();
                var delta = (EndPoint.position - StartPoint.position);
                var length = delta.magnitude;
                var angleVector = delta.normalized;
                var angle = -Mathf.Atan2(angleVector.y, angleVector.x);


                for (int i = 0; i < VertexCount; ++i)
                {
                    float y = Amplitude*Mathf.Sin(i + _progress*Mathf.PI);
                    float x = (length/VertexCount)*i;
                    float screenX = x*Mathf.Cos(angle) + y*Mathf.Sin(angle) + StartPoint.position.x;
                    float screenY = -x*Mathf.Sin(angle) + y*Mathf.Cos(angle) + StartPoint.position.y;
                    _renderer.SetPosition(i, new Vector3(screenX, screenY, StartPoint.position.z));
                }
                for (int i = VertexCount, j = VertexCount; i >= 0; --i,++j)
                {
                    float y = (Amplitude*Mathf.Sin(-(i + _progress*Mathf.PI)));
                    float x = (length/VertexCount)*i;
                    float screenX2 = x*Mathf.Cos(angle) + y*Mathf.Sin(angle) + StartPoint.position.x;
                    float screenY2 = -x*Mathf.Sin(angle) + y*Mathf.Cos(angle) + StartPoint.position.y;
                    _renderer.SetPosition(j, new Vector3(screenX2, screenY2, StartPoint.position.z));
                }
                _progress += (Time.deltaTime*5f);
            }
        }
        
                
        
    }
}
