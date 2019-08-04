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

    [SerializeField]
    private CheckpointClearedEvent OnCheckpointClearedEvent;

    private List<Checkpoint> checkpoints;
    private int currentCheckpointInterval;

    private void Start() {
        checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
        checkpoints.Sort((checkpoint1, checkpoint2) => Math.Sign(checkpoint1.transform.position.x - checkpoint2.transform.position.x));

        List<EnemySpawner> enemySpawners = new List<EnemySpawner>(FindObjectsOfType<EnemySpawner>());
        foreach (EnemySpawner enemySpawner in enemySpawners) {
            enemySpawner.OnPlayerHitEvent.AddListener(Rewind);
        }

        for (int i = 0; i < checkpoints.Count - 1; ++i) {
            Checkpoint lastCheckpoint = checkpoints[i];
            Checkpoint nextCheckpoint = checkpoints[i + 1];
            nextCheckpoint.SetEnemySpawners(enemySpawners.FindAll((enemySpawner) => {
                return lastCheckpoint.transform.position.x < enemySpawner.transform.position.x && enemySpawner.transform.position.x <= nextCheckpoint.transform.position.x;
            }));

            nextCheckpoint.OnCheckpointClearedEvent.AddListener(ClearCurrentCheckpoint);
        }
    }

    void Update() {
        Checkpoint lastCheckpoint = checkpoints[currentCheckpointInterval];
        Checkpoint nextCheckpoint = checkpoints[currentCheckpointInterval + 1];

        if (reference.transform.position.x >= nextCheckpoint.transform.position.x) {
            if (nextCheckpoint.IsCleared()) {
                ClearCurrentCheckpoint(nextCheckpoint);
            } else {
                OnCheckpointEvent?.Invoke(lastCheckpoint.transform.position);
            }
        }
    }

    public void Rewind() {
        Checkpoint lastCheckpoint = checkpoints[currentCheckpointInterval];
        OnCheckpointEvent?.Invoke(lastCheckpoint.transform.position);
    }

    public void ClearCurrentCheckpoint(Checkpoint checkpoint) {
        currentCheckpointInterval++;
        OnCheckpointClearedEvent?.Invoke(checkpoint);
    }
}
