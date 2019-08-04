using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerHitEvent : UnityEvent {
}

public class EnemyScript : MonoBehaviour
{
    public Collider drawingInstanceCollider;
    public Collider attackingCollider;
    public Collider hurtboxCollider;
    public GameObject enemyEntity;

    [SerializeField]
    private PlayerHitEvent OnStartPlayerHitEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other == drawingInstanceCollider)
        {
            //trigger  weapon drawing animation
        }

        if (other == attackingCollider)
        {
            OnStartPlayerHitEvent?.Invoke();
        }

        if (other == hurtboxCollider)
        {
            Destroy(enemyEntity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //trigger sheathing animation
    }
}
