using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;


//@EventHandler - called from MenuSelection.cs
//code for flipping open/close the book in menuscene

public interface IMenuSelectionExit
{
    public void SelectExit(GameObject controller);
}


public class BookInteractionHandler : MonoBehaviour, IMenuSelection, IMenuSelectionExit
{
    //put this on the interactable book

    public static event Action<GameObject> BookInteractEventEnter;
    public static event Action<GameObject> BookInteractEventExit;
    private GameObject interactableBook;
    private HingeJoint bookHinge;
 

    private GameObject cameraRig;
    private bool playVideo, activatedOnce;

    private GameObject currentController;
    private bool isRightController;
    
    private SteamVR_Action_Boolean action;
    private SteamVR_Input_Sources rightSource;
    private SteamVR_Input_Sources leftSource;

    private GameObject tutorial;
    private GameObject video;
    
    //override from IMenuSelection
    public void Select(GameObject controller)
    {
        if (BookInteractEventEnter == null)
            return;
        
        BookInteractEventEnter.Invoke(controller);
        
    }


    // override from IMenuSelectionExit
    public void SelectExit(GameObject controller)
    {
        if (BookInteractEventExit == null)
            return;
        BookInteractEventExit.Invoke(controller);
    }
    

    private void OnEnable()
    {
       BookInteractEventEnter += onBookInteract;
       BookInteractEventExit += onBookInteractExit;
        isRightController = false;
        playVideo = false;
        activatedOnce = false;
        
       tutorial = GameObject.Find("Tutorial");
       video = tutorial.transform.Find("Video").gameObject;

        
        
        interactableBook = GameObject.Find("BookInteractable");
      //  currentController = GameObject.Find("Marker");
        
        if (interactableBook == null)
            Debug.LogError("Could not find the interactable book half in MenuScene. (BookInteractionHandler.cs)");
        else
        {
            bookHinge = interactableBook.GetComponent<HingeJoint>();


        }
    }

    private void OnDisable()
    {
       BookInteractEventEnter -= onBookInteract;
       BookInteractEventExit -= onBookInteractExit;
    }


    // public void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Book interaction enter");
    //     currentController = other.gameObject;
    //     if (currentController.name.ToLower().Contains("right"))
    //     {
    //         isRightController = true;
    //     }
    //     else isRightController = false;
    // }


    // public void OnTriggerExit(Collider other)
    // {
    //     Debug.Log("Book Interaction exit");
    //     currentController = null;
    // }


    public void FixedUpdate()
    {
 
        JointSpring spring = new JointSpring();
        spring.damper = 10;
        spring.spring = 50;


        if (currentController == null)
        {

            if (!activatedOnce)
            {
                spring.targetPosition = -180;
                bookHinge.spring = spring;
                return;
            }

            if (bookHinge.angle > -90)
            {
                playVideo = true;
                video.SetActive(playVideo);
                spring.targetPosition = 0;
            }
            else
            {
                playVideo = false;
                video.SetActive(playVideo);
                spring.targetPosition = -180;
            }
            
            bookHinge.spring = spring;
            return;
        }

        
 
        
        ///constant
        Vector3 globalBookPosition = new Vector3(-1.436f, 0.076f, -0.464f);
        Vector3 globalBookLeftPos = new Vector3(-1.53f, 0.076f, -0.803f);    
        
        Vector3 globalControllerPos = currentController.transform.position;
        
        Vector3 left = globalBookLeftPos - globalBookPosition;
        
        Vector3 deltas = globalControllerPos - globalBookPosition;
        
        double horDist = Math.Sqrt(deltas.x * deltas.x + deltas.z * deltas.z);
        double angle = Math.Atan(deltas.y / horDist);
        
        Vector3 leftNorm = left.normalized;
        Vector3 deltasNorm = deltas.normalized;
        
        float angleDeg = -(float)(angle * 180 / Math.PI);
        float dot = Vector3.Dot(leftNorm, deltasNorm);
        
        
        
        float finalAng = angleDeg;
        
        if (dot < 0)
            finalAng = -69 + (-69 - angleDeg);
        finalAng *= 1.5f;
        
        finalAng = Math.Max(finalAng, -175);
        spring.targetPosition = finalAng;
        bookHinge.spring = spring;
        
        activatedOnce = true;
        

    }


    private void idleSpring(JointSpring spring)
    {
        if (bookHinge.angle > -90)
        {
            spring.targetPosition = 0;
        }
        else
        {
            spring.targetPosition = -180;
        }
            
        bookHinge.spring = spring;

    }


    private void onBookInteractExit(GameObject controller)
    {
        
        Debug.Log("Book Interaction exit");
        currentController = null;
        
    }


   


    private void onBookInteract(GameObject controller)
    {
        Debug.Log("Book interaction enter");
      currentController = controller;
      if (currentController.name.ToLower().Contains("right"))
      {
          isRightController = true;
      }
      else isRightController = false;
      
      
    }
}
