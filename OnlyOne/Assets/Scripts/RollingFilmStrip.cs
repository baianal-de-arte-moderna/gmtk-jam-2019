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
    private Image transitionOverlay;

    [SerializeField]
    private Image rewindOverlay;

    [SerializeField]
    private float transitionSpeed;

    [SerializeField]
    private float speed;

    [SerializeField]
    private FilmStripEvent OnEnterFilmStripEvent;

    [SerializeField]
    private FilmStripEvent OnExitFilmStripEvent;

    private const float inScale = 1;
    private const float outScale = 2;

    private bool isEntering;
    private bool isRewinding;
    private bool shouldExit;
    private bool isExiting;

    private void Start() {
        foreach (Image filmFrame in filmFrames) {
            filmFrame.gameObject.SetActive(false);
        }

        rewindOverlay.gameObject.SetActive(false);
        transitionOverlay.gameObject.SetActive(false);
    }

    private void Update() {
        if (isEntering && transform.localScale.x >= inScale) {
            rewindOverlay.gameObject.SetActive(false);
            transitionOverlay.gameObject.SetActive(true);

            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (transform.localScale.x < inScale) {
                foreach (Image filmFrame in filmFrames) {
                    filmFrame.gameObject.SetActive(true);
                }

                isEntering = false;
                isRewinding = true;
                transform.localScale = inScale * Vector3.one;
                OnEnterFilmStripEvent?.Invoke();
            }
        }

        if (isExiting && transform.localScale.x <= outScale) {
            foreach (Image filmFrame in filmFrames) {
                filmFrame.gameObject.SetActive(false);
            }

            rewindOverlay.gameObject.SetActive(false);
            transitionOverlay.gameObject.SetActive(true);

            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (transform.localScale.x > outScale) {
                rewindOverlay.gameObject.SetActive(false);
                transitionOverlay.gameObject.SetActive(false);

                isExiting = false;
                transform.localScale = outScale * Vector3.one;
                OnExitFilmStripEvent?.Invoke();
            }
        }

        if (isRewinding || shouldExit) {
            rewindOverlay.gameObject.SetActive(true);
            transitionOverlay.gameObject.SetActive(false);

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
