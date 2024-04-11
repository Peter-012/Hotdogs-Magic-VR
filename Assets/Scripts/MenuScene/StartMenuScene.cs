using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        resetGlobal();
        initController();
        initWand();
        initTutorial();
        fadeInView();
    }

    private void resetGlobal() {
        Player1.DominantSide = null;
        Player1.health = 5;
        
        Player2.DominantSide = null;
        Player2.health = 5;
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
