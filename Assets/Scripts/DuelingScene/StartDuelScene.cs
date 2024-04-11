using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.transform;

public class StartDuelScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        initSpawnWand();
        initPlayer();
        initEnemy();
        fadeInView();
    }

    private void initSpawnWand() {
        GameObject leftController;
        GameObject rightController;

        if (Player1.DominantSide.Equals("left")) {
            leftController = GameObject.Find("Controller (left)");
            leftController.AddComponent<SpawnWand>();
        } else if (Player1.DominantSide.Equals("right")) {
            rightController = GameObject.Find("Controller (right)");
            rightController.AddComponent<SpawnWand>();
        } else {
            Debug.LogError("Failed to initialize DominantSide variable from PlayerData.");
        }
    }

    private void initPlayer() {
        GameObject playerObject = GameObject.Find("Camera");
        playerObject.AddComponent<PlayerDamage>();
    }

    private void initEnemy() {
        GameObject.Find("Enemy").AddComponent<EnemyStateManager>(); // For AI Logic
    }

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();
        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
