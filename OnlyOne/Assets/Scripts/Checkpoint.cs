using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CheckpointClearedEvent : UnityEvent {
}

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    public CheckpointClearedEvent OnCheckpointClearedEvent;

    private List<EnemyScript> enemies;

    public void SetEnemies(List<EnemyScript> enemies) {
        this.enemies = enemies;

        foreach (EnemyScript enemy in enemies) {
            enemy.OnEnemyHitEvent.AddListener(OnEnemyHit);
        }
    }

    private void OnEnemyHit(EnemyScript enemy) {
        enemies.Remove(enemy);
        if (enemies.Count == 0) {
            OnCheckpointClearedEvent?.Invoke();
        }
    }
}
