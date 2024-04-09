using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDuelScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    private TransistionScene transition;

    private GameObject leftController;
    private GameObject rightController;
    
    private void Awake() {
        initSpawnWand();
        fadeInView();
    }

    private void initSpawnWand() {
        PlayerData player = Resources.Load<PlayerData>("Player1");

        if (player.DominantSide == "left") {
            leftController = GameObject.Find("Controller (left)");
            leftController.AddComponent<SpawnWand>();
        } else if (player.DominantSide == "right") {
            rightController = GameObject.Find("Controller (right)");
            rightController.AddComponent<SpawnWand>();
        } else {
            Debug.LogError("Failed to initialize DominantSide variable from PlayerData.");
        }
    }

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();
        transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
