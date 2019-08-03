using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RewindEvent : UnityEvent {
}

public class AutoScrollingCamera : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float rewindDuration;

    [SerializeField]
    private RewindEvent OnStartRewindEvent;

    [SerializeField]
    private RewindEvent OnStopRewindEvent;

    private bool isRewinding;
    private float rewindTime;
    private Vector3 rewindSource;
    private Vector3 rewindTarget;

    void Update() {
        if (!isRewinding) {
            transform.position += transform.right * speed * Time.deltaTime;
        } else if (rewindTime > 0) {
            float x = Mathf.Lerp(rewindSource.x, rewindTarget.x, (rewindDuration - rewindTime) / rewindDuration);
            rewindTime -= Time.deltaTime;

            if (rewindTime <= 0) {
                x = rewindTarget.x;
                OnStopRewindEvent?.Invoke();
            }

            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

    public void RewindTo(Vector3 position) {
        if (!isRewinding) {
            isRewinding = true;
            rewindSource = transform.position;
            rewindTarget = position;
            OnStartRewindEvent?.Invoke();
        }
    }

    public void OnEnterFilmStrip() {
        rewindTime = rewindDuration;
    }

    public void OnExitFilmStrip() {
        isRewinding = false;
    }
}
