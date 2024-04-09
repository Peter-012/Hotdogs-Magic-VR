using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tutorial : MonoBehaviour, IMenuSelection {
    public static event Action <GameObject> OnTutorial;
    private bool playVideo = false;

    public void Select(GameObject controller) {
        if (OnTutorial == null) return;
        OnTutorial.Invoke(controller);
    }

    private void OnEnable() {
        OnTutorial += TutorialLogic;
    }

    private void OnDisable() {
        OnTutorial -= TutorialLogic;
    }

    private void TutorialLogic(GameObject controller) {
        playVideo = !playVideo;

        GameObject tutorial = GameObject.Find("Tutorial");
        GameObject video = tutorial.transform.Find("Video").gameObject;
        video.SetActive(playVideo);
    }
}
