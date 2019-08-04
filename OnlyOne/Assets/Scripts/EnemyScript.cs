using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerHitEvent : UnityEvent {
}

[Serializable]
public class EnemyHitEvent : UnityEvent<GameObject> {
}

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private Collider drawingInstanceCollider;

    [SerializeField]
    private Collider attackingCollider;

    [SerializeField]
    private Collider hurtboxCollider;

    [SerializeField]
    private PlayerHitEvent OnPlayerHitEvent;

    [SerializeField]
    private EnemyHitEvent OnEnemyHitEvent;

    private void OnTriggerEnter(Collider other) {
        if (other == drawingInstanceCollider) {
            //trigger  weapon drawing animation
        }

        if (other == attackingCollider) {
            OnPlayerHitEvent?.Invoke();
        }

        if (other == hurtboxCollider) {
            OnEnemyHitEvent?.Invoke(gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == drawingInstanceCollider) {
            //trigger sheathing animation
        }
    }
}
