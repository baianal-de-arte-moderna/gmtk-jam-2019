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
    public PlayerHitEvent OnPlayerHitEvent;

    [SerializeField]
    public EnemyHitEvent OnEnemyHitEvent;

    public void OnEnemyDrawingRangeEnter() {
        //trigger  weapon drawing animation
    }

    public void OnEnemyDrawingRangeExit() {
        //trigger sheathing animation
    }

    public void OnEnemyAttackingRangeEnter() {
        OnPlayerHitEvent?.Invoke();
    }

    public void OnEnemyHurtBoxHit() {
        OnEnemyHitEvent?.Invoke(this);
    }
}
