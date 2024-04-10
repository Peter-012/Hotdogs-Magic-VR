using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDuelScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        initSpawnWand();
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

    private void initEnemy() {
        GameObject enemyObject = GameObject.Find("Enemy");
        enemyObject.AddComponent<EnemyStateManager>(); // For AI Logic
        enemyObject.AddComponent<EnemyDamageLogic>();
    }

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();
        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
