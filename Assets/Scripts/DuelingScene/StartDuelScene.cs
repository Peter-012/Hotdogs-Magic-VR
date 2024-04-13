using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.transform;

public class StartDuelScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private string musicPath = "duelingSong";
    
    private void Awake() {
        initSpawnWand();
        initPlayer();
        initEnemy();
        fadeInView();
        Game.startGame = true;
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
        GameObject enemyObject = GameObject.Find("Enemy");

        // Make sure the spawn point of enemy aligns with player
        Vector3 enemyPosition = enemyObject.transform.position;
        enemyPosition.z = GameObject.Find("[CameraRig]").transform.position.z;
        enemyObject.transform.position = enemyPosition;

        // Save position of the enemy spawnpoint
        Player2.spawnPoint = enemyPosition;

        // Add AI Logic Component
        enemyObject.AddComponent<EnemyStateManager>();
    }

    private void fadeInView() {
        // Background Dueling Music
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = Resources.Load<AudioClip>(musicPath);
        audioSource.Play();

        // Fade In Player View
        gameObject.AddComponent<TransistionScene>();
        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
