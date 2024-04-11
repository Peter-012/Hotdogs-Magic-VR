using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScene : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 1.0f;
    
    private void Awake() {
        initController();
        initWand();
        initTutorial();
        fadeInView();
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
        tutorialObject.AddComponent<Tutorial>();

        GameObject dynamicBook = GameObject.Find("BookInteractable");
        initBook(tutorialObject, dynamicBook);

        
        
        GameObject tutorialVideo = GameObject.Find("Video");
        tutorialVideo.AddComponent<Video>();
        tutorialVideo.SetActive(false);
    }

    private void initBook(GameObject staticBook, GameObject dynamicBook)
    {
        
/*
 *
anchor 0.013, 0, 0.0045
axis: 0,1,0
connected anchor: -1.430706   0.05706573   -0.4633129
 */


       dynamicBook.AddComponent<HingeJoint>();
       HingeJoint joint = dynamicBook.GetComponent<HingeJoint>();
        joint.anchor = new Vector3(0.013f, 0f, 0.0045f);
        joint.axis = new Vector3(0, 1, 0);
        joint.connectedAnchor = new Vector3(-1.4307f, 0.0576f, -0.463f);
       
        joint.useSpring = true;
        JointSpring spring = new JointSpring();
        spring.spring = 5000;  ///newtons
        joint.useLimits = true;
        joint.spring = spring;

        JointLimits lim = new JointLimits();
        lim.min = -175;
        lim.max = -360;
        joint.limits = lim;
       
       dynamicBook.AddComponent<BookInteractionHandler>();

    }
    
    
    
    

    private void fadeInView() {
        gameObject.AddComponent<TransistionScene>();

        TransistionScene transition = FindObjectOfType<TransistionScene>();
        transition.fadeInView(fadeInDuration);
    }
}
