using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CheckpointEvent : UnityEvent<Vector3> {
}

public class Rewinder : MonoBehaviour {

    [SerializeField]
    private GameObject reference;

    [SerializeField]
    private GameObject lastCheckpoint;

    [SerializeField]
    private GameObject nextCheckpoint;

    [SerializeField]
    private CheckpointEvent OnCheckpointEvent;

    void Update() {
        if (reference.transform.position.x >= nextCheckpoint.transform.position.x) {
            OnCheckpointEvent?.Invoke(lastCheckpoint.transform.position);
        }
    }
}
