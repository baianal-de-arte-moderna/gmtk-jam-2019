using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnemyTriggerEnterEvent : UnityEvent {
}

[Serializable]
public class EnemyTriggerExitEvent : UnityEvent {
}

public class EnemyTrigger : MonoBehaviour {

    [SerializeField]
    private EnemyTriggerEnterEvent OnEnemyTriggerEnterEvent;

    [SerializeField]
    private EnemyTriggerExitEvent OnEnemyTriggerExitEvent;

    private void OnTriggerEnter(Collider other) {
        OnEnemyTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        OnEnemyTriggerExitEvent?.Invoke();
    }
}
