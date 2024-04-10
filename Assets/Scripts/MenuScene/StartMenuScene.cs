using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        initController();
        initWand();
        initTutorial();
        fadeInView();
    }

    private void initController() {
        GameObject leftController = GameObject.Find("Controller (left)");
        leftController.AddComponent<MenuSelection>();

        GameObject rightController = GameObject.Find("Controller (right)");
        rightController.AddComponent<MenuSelection>();
    }

    private void initWand() {
        GameObject wandObject = GameObject.Find("Wand");
        wandObject.AddComponent<StartDuel>();
    }

    private void initTutorial() {
        GameObject tutorialObject = GameObject.Find("BookTutorial");
        tutorialObject.AddComponent<Tutorial>();

        GameObject tutorialVideo = GameObject.Find("Video");
        tutorialVideo.AddComponent<Video>();
        tutorialVideo.SetActive(false);
    }

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();

        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
