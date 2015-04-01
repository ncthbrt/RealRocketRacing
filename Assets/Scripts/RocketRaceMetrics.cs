using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class RocketRaceMetrics : MonoBehaviour
    {

        private Checkpoint _currentCheckpoint;
        public int LapCount
        {
            get; private set; 
        }

        public float RespawnTime = 4f;
        public TimeSpan TotalTime { get; private set; }

        public TimeSpan[] LapTimes { get; private set; }

        public TimeSpan CurrentLapTime
        {
            get { return LapTimes[LapCount]; }
        }


        public int NumberOfLaps;    
        public Checkpoint CurrentCheckpoint
        {
            get { return _currentCheckpoint; }
            private set { _currentCheckpoint = value; }
        }
        public Checkpoint StartingCheckpoint;

        public int NumberOfCheckpoints;


        public delegate void LapComplete(GameObject rocket,TimeSpan lapTime,int lapNumber);
        public delegate void PassedCheckpoint(Checkpoint checkpoint, GameObject rocket);


        private LapComplete _lapCompleteCallbacks;
        private PassedCheckpoint _passedCheckpointCallbacks;

        public void AddLapCompleteCallback(LapComplete complete){
            _lapCompleteCallbacks+=complete;
        }

        public void AddPassedCheckpointCallback(PassedCheckpoint complete){
            _passedCheckpointCallbacks+=complete;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            
            var checkpoint=other.GetComponentInParent<Checkpoint>();            
            if (checkpoint != null && (_currentCheckpoint == null || checkpoint.CheckpointID == _currentCheckpoint.CheckpointID + 1 || (_currentCheckpoint.CheckpointID == NumberOfCheckpoints - 1 && checkpoint.CheckpointID == 0)))
            {
                _currentCheckpoint=checkpoint;                            
                if(_currentCheckpoint.CheckpointID==0){//If the agent has completed a lap
                    
                    if (_lapCompleteCallbacks != null) { 
                        _lapCompleteCallbacks(gameObject,CurrentLapTime,LapCount);                                                
                    }
                    ++LapCount;                    
                }else if(_passedCheckpointCallbacks!=null){
                    _passedCheckpointCallbacks(_currentCheckpoint,gameObject);
                }
            }
        }


        // Use this for initialization
        void Start () {
            _currentCheckpoint = StartingCheckpoint;
            TotalTime = new TimeSpan();
            LapTimes = new TimeSpan[NumberOfLaps];
            LapCount = 0;
        }
	
        // Update is called once per frame
        void Update () {
            var delta = TimeSpan.FromSeconds(Time.deltaTime);
            TotalTime += delta;
            LapTimes[LapCount] += delta;
        }

        public void ToCurrentCheckpoint()
        {
            var checkpoint = CurrentCheckpoint;            
            rigidbody2D.position = checkpoint.Location;
            rigidbody2D.rotation = (checkpoint.Heading);
            rigidbody2D.angularVelocity = 0;
            rigidbody2D.velocity = new Vector2(0, 0);
        }
    }
}
