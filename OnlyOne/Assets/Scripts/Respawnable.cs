using UnityEngine;

public class Respawnable : MonoBehaviour {

    private Vector3 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    public void Despawn() {
        gameObject.SetActive(false);
    }

    public void Respawn() {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
