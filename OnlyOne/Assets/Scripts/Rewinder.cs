using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CheckpointEvent : UnityEvent<Vector3> {
}

public class Rewinder : MonoBehaviour {

    [SerializeField]
    private GameObject reference;

    [SerializeField]
    private CheckpointEvent OnCheckpointEvent;

    private List<Checkpoint> checkpoints;
    private int currentCheckpointInterval;

    private void Start() {
        checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
        checkpoints.Sort((checkpoint1, checkpoint2) => Math.Sign(checkpoint1.transform.position.x - checkpoint2.transform.position.x));
    }

    void Update() {
        Checkpoint lastCheckpoint = checkpoints[currentCheckpointInterval];
        Checkpoint nextCheckpoint = checkpoints[currentCheckpointInterval + 1];

        if (reference.transform.position.x >= nextCheckpoint.transform.position.x) {
            OnCheckpointEvent?.Invoke(lastCheckpoint.transform.position);
        }
    }

    public void ClearCurrentCheckpoint() {
        currentCheckpointInterval++;
    }
}
