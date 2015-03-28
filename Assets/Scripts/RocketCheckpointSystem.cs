using UnityEngine;
using System.Collections;

public class RocketCheckpointSystem : MonoBehaviour {
	private const int LAST_CHECKPOINT=-1; /*The use of -1 is used to indicate the last checkpoint in a lap. 
										   * This is convenient, in that the use of means that you don't have 
										   * manually set gating parameters for each checkpoint. The last 
										   * and first checkpoints are the ones with the special conditions.
										   * The other checkpoints follow an ascendant rule.
										   */
									
	public Checkpoint CurrentCheckpoint{ get;  private set; }
	public Checkpoint StartingCheckpoint{get;  set;}



	public delegate void LapComplete(GameObject rocket);
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
		if (checkpoint!=null && (CurrentCheckpoint==null || checkpoint.CheckpointID>CurrentCheckpoint.CheckpointID || (CurrentCheckpoint.CheckpointID!=0 && checkpoint.CheckpointID==LAST_CHECKPOINT)))
		{
			CurrentCheckpoint=checkpoint;            
			Debug.Log("Current Checkpoint: "+CurrentCheckpoint.CheckpointID);
			if(CurrentCheckpoint.CheckpointID==0 && _lapCompleteCallbacks!=null){//If the agent has completed a lap
				_lapCompleteCallbacks(gameObject);
			}else if(_passedCheckpointCallbacks!=null){
				_passedCheckpointCallbacks(CurrentCheckpoint,gameObject);
			}
		}
	}


	// Use this for initialization
	void Start () {
		CurrentCheckpoint = StartingCheckpoint;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
