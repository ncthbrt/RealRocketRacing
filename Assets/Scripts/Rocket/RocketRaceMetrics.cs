using System;
using UnityEngine;
using RealRocketRacing.RaceCheckpoints;
namespace RealRocketRacing.Rocket
{
    public class RocketRaceMetrics : MonoBehaviour
    {

        private Checkpoint _currentCheckpoint;
        public int LapCount
        {
            get; private set; 
        }

        public GameObject VictoryInfo;

		private bool _running = false;
		public void StartTimer(){
			_running = true;
		}

		public void StopTimer(){
			_running = false;
		}
		public bool TimerRunning {
			get{
				return _running;
			}
		}

        public float RespawnTime = 4f;
        public TimeSpan TotalTime { get; private set; }

        public TimeSpan[] LapTimes { get; private set; }

        public TimeSpan CurrentLapTime
        {
            get {
                if (LapCount < LapTimes.Length)
                {
                    return LapTimes[LapCount];
                }
                else
                {
                    return LapTimes[LapTimes.Length-1];
                }
            }
        }


        public int NumberOfLaps;    
        public Checkpoint CurrentCheckpoint
        {
            get { return _currentCheckpoint!=null?_currentCheckpoint:StartingCheckpoint; }
            private set { _currentCheckpoint = value; }
        }
        public Checkpoint StartingCheckpoint;

        public int NumberOfCheckpoints;


        public delegate void LapComplete(GameObject rocket,TimeSpan lapTime,int lapNumber);
        public delegate void PassedCheckpoint(Checkpoint checkpoint, GameObject rocket);


        private LapComplete _lapCompleteCallbacks;
        private PassedCheckpoint _passedCheckpointCallbacks;
		private PassedCheckpoint _passedInvalidCheckpointCallback;

        public void AddLapCompleteCallback(LapComplete complete){
            _lapCompleteCallbacks+=complete;
        }

        public void AddPassedCheckpointCallback(PassedCheckpoint callback){
            _passedCheckpointCallbacks+=callback;
        }

		public void AddPassedInvalidCheckpointCallback(PassedCheckpoint callback){
			_passedInvalidCheckpointCallback += callback;
		}

        void OnTriggerEnter2D(Collider2D other)
        {            
            var checkpoint=other.GetComponentInParent<Checkpoint>();            
            if (checkpoint != null)
            {
				if(checkpoint.CheckpointID == (_currentCheckpoint.CheckpointID + 1)%NumberOfCheckpoints){
	                _currentCheckpoint=checkpoint;                            
	                if(_currentCheckpoint.CheckpointID==0){//If the agent has completed a lap	                    
	                    if (_lapCompleteCallbacks != null) { 
	                        _lapCompleteCallbacks(gameObject,CurrentLapTime,LapCount);                                                
	                    }
	                    ++LapCount;                    
	                }

					if(_passedCheckpointCallbacks!=null){
	                    _passedCheckpointCallbacks(_currentCheckpoint,gameObject);
	                }
				}else if(checkpoint.CheckpointID!=CurrentCheckpoint.CheckpointID){ //Passed through invalid checkpoint
					_passedInvalidCheckpointCallback(checkpoint,gameObject);
				}
            }
        }


        // Use this for initialization
        void Start () {
            _currentCheckpoint = StartingCheckpoint;
            TotalTime = new TimeSpan();
            LapTimes = new TimeSpan[NumberOfLaps];
            LapCount = 0;
			AddLapCompleteCallback (TrackComplete);
        }
		private void TrackComplete(GameObject rocket,TimeSpan lapTime,int lapNumber){			            
			if (lapNumber == NumberOfLaps - 1)
			{
                Debug.Log("Track Complete");
                var victoryInfo = GameObject.Instantiate(VictoryInfo);
			    var victoryPallette=victoryInfo.GetComponent<RocketPalette>();
			    var pallette = GetComponent<RocketPalette>();
			    victoryPallette.BaseColor = pallette.BaseColor;
			    victoryPallette.AccentColor = pallette.AccentColor;
			    var vinfo=victoryInfo.GetComponent<VictoryInfo>();
			    vinfo.RocketRigidBody = GetComponent<Rigidbody2D>();
			    vinfo.Time = this.TotalTime;
			    vinfo.WinningPlayer = GetComponent<RocketController>().RocketPlayer;                
			}
		}        
        // Update is called once per frame
        void Update () {
			if (_running) {
				var delta = TimeSpan.FromSeconds (Time.deltaTime);
				TotalTime += delta;
                if (LapCount < LapTimes.Length) { 
				    LapTimes [LapCount] += delta;
                }
			}
        }

        public void ToCurrentCheckpoint()
        {
			var checkpoint = CurrentCheckpoint;
            GetComponent<Rigidbody2D>().rotation = (checkpoint.Heading);            
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().velocity =  Vector2.zero;
            GetComponent<Transform>().position = checkpoint.Location;         
        }
    }
}
