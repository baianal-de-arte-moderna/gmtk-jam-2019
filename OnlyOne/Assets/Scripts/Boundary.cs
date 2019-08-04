using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BoundaryTriggerEvent : UnityEvent {
}

public class Boundary : MonoBehaviour {

    [SerializeField]
    private BoundaryTriggerEvent OnBoundaryTriggerEvent;

    private void OnTriggerEnter(Collider other) {
        OnBoundaryTriggerEvent?.Invoke();
    }
}
