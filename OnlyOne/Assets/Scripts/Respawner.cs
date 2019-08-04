using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {

    private List<Respawnable> respawnables;
    private int currentCheckpointInterval;

    private void Start() {
        respawnables = new List<Respawnable>(FindObjectsOfType<Respawnable>());
    }

    public void Respawn() {

    }
}
