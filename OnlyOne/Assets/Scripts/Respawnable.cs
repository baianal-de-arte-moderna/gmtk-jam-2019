using UnityEngine;

public class Respawnable : MonoBehaviour {

    protected Vector3 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    public void ClearCheckpoint(Checkpoint checkpoint) {
        initialPosition = checkpoint.transform.position;
    }

    public void Despawn() {
        gameObject.SetActive(false);
    }

    public void Respawn() {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
