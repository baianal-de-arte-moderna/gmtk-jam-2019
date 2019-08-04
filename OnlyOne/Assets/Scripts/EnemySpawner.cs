using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    private GameObject enemyInstance;

    private void Start() {
        Spawn();
    }

    public void Spawn() {
        if (!enemyInstance) {
            enemyInstance = Instantiate(enemyPrefab, transform);
        }
    }
}
