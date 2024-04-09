using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartDuel : MonoBehaviour, IMenuSelection {
    [SerializeField] private float fadeOutDuration = 3.0f;

    public static event Action <GameObject> OnStartDuel;

    public void Select(GameObject controller) {
        if (OnStartDuel == null) return;
        OnStartDuel.Invoke(controller);
    }

    private void OnEnable() {
        OnStartDuel += StartDuelLogic;
    }

    private void OnDisable() {
        OnStartDuel -= StartDuelLogic;
    }

    private void StartDuelLogic(GameObject controller) {
        // // Attach wand to hand
        gameObject.AddComponent<FixedJoint>();
        FixedJoint joint = gameObject.GetComponent<FixedJoint>();
        joint.connectedBody = controller.GetComponent<Rigidbody>();
        joint.breakForce = Mathf.Infinity;

        // Position wand to fit in hand
        // wandObject.transform.position = transform.position;
        // wandObject.transform.Rotate(70f, 0, 0);

        PlayerData player = Resources.Load<PlayerData>("Player1");

        if (controller.name.Contains("left")) {
            player.DominantSide = "left";
        } else if (controller.name.Contains("right")) {
            player.DominantSide = "right";
        } else {
            Debug.LogError("Failed to initialize DominantSide variable from PlayerData.");
        }

        // Load "DuelingScene" while fading user screen
        GameObject cameraRig = GameObject.Find("[CameraRig]");
        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeOutToScene(fadeOutDuration, "DuelingScene");
    }
}
