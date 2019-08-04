using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {

    private List<Respawnable> respawnables;

    private void Start() {
        respawnables = new List<Respawnable>(FindObjectsOfType<Respawnable>());
    }

    public void Respawn() {
        foreach (Respawnable respawnable in respawnables) {
            if (respawnable.enabled) {
                respawnable.Respawn();
            }
        }

        List<EnemySpawner> enemySpawners = new List<EnemySpawner>(FindObjectsOfType<EnemySpawner>());
        foreach (EnemySpawner enemySpawner in enemySpawners) {
            enemySpawner.Spawn();
        }
    }
}
