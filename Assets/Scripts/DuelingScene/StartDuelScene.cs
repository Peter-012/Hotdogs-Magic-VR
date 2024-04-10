using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDuelScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        // PlayerData player = Resources.Load<PlayerData>("Player1"); //TESTING ONLY
        // player.DominantSide = "right"; //TESTING ONLY

        initSpawnWand();
        initEnemy();
        fadeInView();
    }

    private void initSpawnWand() {
        PlayerData player = Resources.Load<PlayerData>("Player1");
        GameObject leftController;
        GameObject rightController;

        if (player.DominantSide.Equals("left")) {
            leftController = GameObject.Find("Controller (left)");
            leftController.AddComponent<SpawnWand>();
        } else if (player.DominantSide.Equals("right")) {
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
