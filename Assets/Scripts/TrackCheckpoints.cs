using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    private GameController gameController;
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointSingleIndex;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform CheckpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = CheckpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;

        if(this.checkpointSingleList.Count > 0) {
            getLastCheckPoint().gameObject.SetActive(false);
        }

    }

    public void Start() {
        this.gameController = FindObjectOfType<GameController>();
    }


    public void PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        int checkpointIndex = checkpointSingleList.IndexOf(checkpointSingle);
        if (checkpointIndex == nextCheckpointSingleIndex) {
            Debug.Log("Correct");
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            if (checkpointIndex == 0)
                getLastCheckPoint().gameObject.SetActive(true);
            if(checkpointIndex == checkpointSingleList.Count - 1) {
                this.gameController.finishRace();
            }
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            //Wrong checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);
        }
    }

    public CheckpointSingle getLastCheckPoint() {
        return this.checkpointSingleList[this.checkpointSingleList.Count - 1];
    }

}
