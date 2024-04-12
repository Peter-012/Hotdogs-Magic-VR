using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public interface IMenuSelection {
    public void Select(GameObject controller);
}

public class MenuSelection : MonoBehaviour {
    private SteamVR_Input_Sources InputSource;
    public static SteamVR_Action_Boolean ActionBoolean;   //kinda need it to be public sorry

    private Rigidbody controllerRigid;
    private BoxCollider collisionBox;

    private GameObject controller;
    private IMenuSelection MenuOption;

    //for whether the controller is right/left hand
    private bool isRight;
    

    private void Start() {
        initSteamVR();
        addComponents();
    }

    private void Update() {
        if (ActionBoolean.GetStateDown(InputSource)) {
            //set clench true here depending on hand

            if (isRight)
            {
                SteamVR_Behaviour_Skeleton.isRightClenching = true;
            }
            else
            {
                SteamVR_Behaviour_Skeleton.isLeftClenching = true;
            }
            
            if (MenuOption == null) 
                return;
            MenuOption.Select(gameObject);
            Debug.Log("Select action fired");
        }
        else if (ActionBoolean.GetStateUp(InputSource))
        {
            
            //this is for the glove mechanics
            if (isRight)
            {
                SteamVR_Behaviour_Skeleton.isRightClenching = false;
            }
            else
            {
                SteamVR_Behaviour_Skeleton.isLeftClenching = false;
            }
            
            
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
            isRight = false;
        }
        else if (gameObject.name.Contains("right")) {
            InputSource = SteamVR_Input_Sources.RightHand;
            isRight = true;
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
