using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerHitEvent : UnityEvent {
}

[Serializable]
public class EnemyHitEvent : UnityEvent<EnemyScript> {
}

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private Collider drawingInstanceCollider;

    [SerializeField]
    private Collider attackingCollider;

    [SerializeField]
    private Collider hurtboxCollider;

    [SerializeField]
    public PlayerHitEvent OnPlayerHitEvent;

    [SerializeField]
    public EnemyHitEvent OnEnemyHitEvent;

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider == hurtboxCollider) {
            OnEnemyHitEvent?.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other == drawingInstanceCollider) {
            //trigger  weapon drawing animation
        }

        if (other == attackingCollider) {
            OnPlayerHitEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == drawingInstanceCollider) {
            //trigger sheathing animation
        }
    }
}
