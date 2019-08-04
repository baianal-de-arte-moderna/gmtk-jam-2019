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

        if (IsCleared()) {
            OnCheckpointClearedEvent?.Invoke(this);
        }
    }

    public bool IsCleared() {
        List<EnemySpawner> aliveEnemySpawners = enemySpawners.FindAll(EnemySpawner.IsSpawned);
        return aliveEnemySpawners.Count == 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
