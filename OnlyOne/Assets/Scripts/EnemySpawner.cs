using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnemySpawnerHitEvent : UnityEvent<EnemySpawner> {
}

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    public PlayerHitEvent OnPlayerHitEvent;
    public EnemySpawnerHitEvent OnEnemySpawnerHitEvent;

    private GameObject enemyInstance;

    private void Start() {
        Spawn();
    }

    public void Spawn() {
        if (!IsSpawned()) {
            enemyInstance = Instantiate(enemyPrefab, transform);

            EnemyScript enemyScript = enemyInstance.GetComponent<EnemyScript>();
            if (enemyScript) {
                enemyScript.OnPlayerHitEvent.AddListener(() => OnPlayerHitEvent?.Invoke());
                enemyScript.OnEnemyHitEvent.AddListener((enemy) => OnEnemySpawnerHitEvent?.Invoke(this));
            }
        }
    }

    public void Despawn() {
        Destroy(enemyInstance);
        enemyInstance = null;
    }

    public static bool IsSpawned(EnemySpawner enemySpawner) {
        return enemySpawner.IsSpawned();
    }

    private bool IsSpawned() {
        return enemyInstance;
    }
}
