using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class FilmStripEvent : UnityEvent {
}

public class RollingFilmStrip : MonoBehaviour {

    [SerializeField]
    private List<Image> filmFrames;

    [SerializeField]
    private float transitionSpeed;

    [SerializeField]
    private float speed;

    [SerializeField]
    private FilmStripEvent OnEnterFilmStripEvent;

    [SerializeField]
    private FilmStripEvent OnExitFilmStripEvent;

    private bool isEntering;
    private bool isRewinding;
    private bool shouldExit;
    private bool isExiting;

    private void Update() {
        if (isEntering && transform.localScale.x >= 1) {
            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (transform.localScale.x < 1) {
                isEntering = false;
                isRewinding = true;
                transform.localScale = 1 * Vector3.one;
                OnEnterFilmStripEvent?.Invoke();
            }
        }

        if (isExiting && transform.localScale.x <= 2) {
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (transform.localScale.x > 2) {
                isExiting = false;
                transform.localScale = 2 * Vector3.one;
                OnExitFilmStripEvent?.Invoke();
            }
        }

        if (isRewinding || shouldExit) {
            for (int i = 0; i < filmFrames.Count; ++i) {
                Image filmFrame = filmFrames[i];
                float y = filmFrame.rectTransform.anchoredPosition.y;
                if (y < 0) {
                    Image nextFilmFrame = filmFrames[(i + 1) % filmFrames.Count];
                    y += nextFilmFrame.rectTransform.anchoredPosition.y + nextFilmFrame.sprite.rect.height;

                    if (shouldExit) {
                        shouldExit = false;
                        isExiting = true;
                    }
                }
                y -= speed * Time.deltaTime;
                filmFrame.rectTransform.anchoredPosition = new Vector2(filmFrame.rectTransform.anchoredPosition.x, y);
            }
        }
    }

    public void StartRewind() {
        isEntering = true;
        isRewinding = false;
    }

    public void StopRewind() {
        shouldExit = true;
        isRewinding = false;
    }
}
