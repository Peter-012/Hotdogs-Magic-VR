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
        GameObject handRight = GameObject.Find("vr_glove_right");
    //    GameObject middleRight = GameObject.Find("vr_glove_right/vr_glove_model/Root/finger_middle_r_aux");



    gameObject.transform.position = handRight.transform.position;  ////
        
        FixedJoint joint = gameObject.GetComponent<FixedJoint>();
        joint.connectedBody = controller.GetComponent<Rigidbody>();
        joint.breakForce = Mathf.Infinity;

        // Position wand to fit in hand
        // wandObject.transform.position = transform.position;
        // wandObject.transform.Rotate(70f, 0, 0);

        if (controller.name.Contains("left")) {
            Player1.DominantSide = "left";
        } else if (controller.name.Contains("right")) {
            Player1.DominantSide = "right";
        } else {
            Debug.LogError("Failed to initialize DominantSide variable from PlayerData.");
        }
        
        // Load "DuelingScene" while fading user screen
        GameObject cameraRig = GameObject.Find("[CameraRig]");
        TransistionScene transition = FindObjectOfType<TransistionScene>();
     //   transition.fadeOutToScene(fadeOutDuration, "DuelingScene");
    }
}
