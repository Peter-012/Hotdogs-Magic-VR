using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


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
    private Rigidbody bookRigidBody;
    private HingeJoint bookHinge;
    private bool applyForce;

    private GameObject cameraRig;

    private GameObject currentController;
    
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
        applyForce = false;
        
        interactableBook = GameObject.Find("BookInteractable");
      //  currentController = GameObject.Find("Marker");
        
        if (interactableBook == null)
            Debug.LogError("Could not find the interactable book half in MenuScene. (BookInteractionHandler.cs)");
        else
        {
            bookRigidBody = interactableBook.GetComponent<Rigidbody>();
            bookHinge = interactableBook.GetComponent<HingeJoint>();


        }
        
    }

    private void OnDisable()
    {
        BookInteractEventEnter -= onBookInteract;
        BookInteractEventExit -= onBookInteractExit;
    }

    public void FixedUpdate()
    {
        
        if (currentController == null)
            return;

        Rigidbody body = currentController.GetComponent<Rigidbody>();
       

        
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
        
        JointLimits lims = new JointLimits();
        lims.max = finalAng;
        lims.min = -360;
        
        bookHinge.limits = lims;



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
    }
}
