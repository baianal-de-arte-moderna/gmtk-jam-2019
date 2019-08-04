using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CheckpointClearedEvent : UnityEvent<Checkpoint> {
}

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    public CheckpointClearedEvent OnCheckpointClearedEvent;

    private List<EnemySpawner> enemySpawners;

    public void SetEnemySpawners(List<EnemySpawner> enemySpawners) {
        this.enemySpawners = enemySpawners;

        foreach (EnemySpawner enemySpawner in enemySpawners) {
            enemySpawner.OnEnemySpawnerHitEvent.AddListener(OnEnemySpawnerHit);
            OnCheckpointClearedEvent.AddListener((Checkpoint checkpoint) => {
                Destroy(enemySpawner);
            });
        }
    }

    private void OnEnemySpawnerHit(EnemySpawner enemySpawner) {
        enemySpawner.Despawn();

        List<EnemySpawner> aliveEnemySpawners = enemySpawners.FindAll(EnemySpawner.IsSpawned);
        if (aliveEnemySpawners.Count == 0) {
            OnCheckpointClearedEvent?.Invoke(this);
        }
    }
}
