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

    private List<GameObject> checkpoints;
    private int currentCheckpointInterval;

    private void Start() {
        checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        checkpoints.Sort((checkpoint1, checkpoint2) => Math.Sign(checkpoint1.transform.position.x - checkpoint2.transform.position.x));
    }

    void Update() {
        GameObject lastCheckpoint = checkpoints[currentCheckpointInterval];
        GameObject nextCheckpoint = checkpoints[currentCheckpointInterval + 1];

        if (reference.transform.position.x >= nextCheckpoint.transform.position.x) {
            OnCheckpointEvent?.Invoke(lastCheckpoint.transform.position);
        }
    }

    public void ClearCurrentCheckpoint() {
        currentCheckpointInterval++;
    }
}
