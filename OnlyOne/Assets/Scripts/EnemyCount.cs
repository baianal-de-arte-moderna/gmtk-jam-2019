using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour {

    private Text text;

    private void Start() {
        text = GetComponent<Text>();
    }

    public void UpdateEnemyCount(Checkpoint checkpoint) {
        List<EnemySpawner> enemySpawners = checkpoint.GetEnemySpawners();
        List<EnemySpawner> aliveEnemySpawners = enemySpawners.FindAll(EnemySpawner.IsSpawned);
        text.text = $"{aliveEnemySpawners.Count} / {enemySpawners.Count}";
    }
}
