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
       // tutorialObject.AddComponent<Tutorial>();  don't put this on there else it will break

        GameObject dynamicBook = GameObject.Find("BookInteractable");
        initBook(tutorialObject, dynamicBook);
        
        

        GameObject tutorialVideo = GameObject.Find("Video");
        tutorialVideo.AddComponent<Video>();
        tutorialVideo.SetActive(false);
    }
    
    private void initBook(GameObject staticBook, GameObject dynamicBook)
    {

        staticBook.AddComponent<ArticulationBody>();
        ArticulationBody body = staticBook.GetComponent<ArticulationBody>();
        body.useGravity = false;
        body.immovable = true;
        
        
        dynamicBook.AddComponent<HingeJoint>();
        HingeJoint joint = dynamicBook.GetComponent<HingeJoint>();
        joint.anchor = new Vector3(0.013f, 0f, 0.0045f);
        joint.axis = new Vector3(0, 1, 0);
        joint.connectedAnchor = new Vector3(-1.4307f, 0.0576f, -0.463f);
        joint.connectedArticulationBody = body;
        
        joint.useSpring = true;
        JointSpring spring = new JointSpring();
        spring.spring = 500;  ///newtons
        spring.damper = 100;
        spring.targetPosition = -180;

        joint.spring = spring;

        BoxCollider collider = dynamicBook.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        
        
        
        dynamicBook.AddComponent<BookInteractionHandler>();

    }
    
    
    
    

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();

        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
