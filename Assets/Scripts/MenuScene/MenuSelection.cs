using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public interface IMenuSelection {
    public void Select(GameObject controller);
}

public class MenuSelection : MonoBehaviour {
    private SteamVR_Input_Sources InputSource;
    private SteamVR_Action_Boolean ActionBoolean;

    private Rigidbody controllerRigid;
    private BoxCollider collisionBox;

    private GameObject controller;
    private IMenuSelection MenuOption;

    private void Start() {
        initSteamVR();
        addComponents();
    }

    private void Update() {
        if (ActionBoolean.GetStateDown(InputSource)) {
            if (MenuOption == null) return;
            MenuOption.Select(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        MenuOption = collision.GetComponent<IMenuSelection>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (MenuOption != null)
        {
           IMenuSelectionExit exitEvent = other.GetComponent<IMenuSelectionExit>();
           if (exitEvent == null)
           {
               MenuOption = null;
               return;
           }
           
           //where gameObject is the controller
           exitEvent.SelectExit(gameObject);
        }
        
        MenuOption = null;
    }

    private void initSteamVR() {
        // Initialize InputSource
        if (gameObject.name.Contains("left")) {
            InputSource = SteamVR_Input_Sources.LeftHand;
        }
        else if (gameObject.name.Contains("right")) {
            InputSource = SteamVR_Input_Sources.RightHand;
        }
        else {
            Debug.LogError("Failed to initialize SteamVR input source.");
        }

        // Initialize ActionBoolean
        ActionBoolean = SteamVR_Input
            .GetActionFromPath<SteamVR_Action_Boolean>("/actions/default/in/GrabGrip");
    }

    private void addComponents() {
        // Create hands as rigidbody
        if (gameObject.GetComponent<Rigidbody>() == null) {
            controllerRigid = gameObject.AddComponent<Rigidbody>();
        } else {
            controllerRigid = gameObject.GetComponent<Rigidbody>();
        }
        controllerRigid.useGravity = false;
        controllerRigid.isKinematic = true;

        // Add collision box to hands
        if (gameObject.GetComponent<BoxCollider>() == null) {
            collisionBox = gameObject.AddComponent<BoxCollider>();
        } else {
            collisionBox = gameObject.GetComponent<BoxCollider>();
        }
        collisionBox.isTrigger = true;
        collisionBox.center = new Vector3(0f, 0f, -0.08f);
        collisionBox.size = new Vector3(0.12f, 0.12f, 0.12f);
    }
}
