using System;
using System.Collections;
using System.Collections.Generic;
using DuelingScene.Entity;
using UnityEngine;
using Valve.VR;

//
//Used on the potion objects
//listens to interaction events
public class PotionInteractionLogic : MonoBehaviour, IMenuSelection, IOnSelectionRelease
{
    
    
    //passes in the controller which entered the trigger
    //gameObject is the actual Potion object

    private bool isHoming;
    public static bool continueSpawning = true;
    //this is primarily a flag to tell the spawner to stop spawning until the player throws the thing
    //this also serves as a flag for selection too - if there already is a selection we should release
    //it first before selecting a different potion
    
    
    private bool wasSelected;
    private bool wasReleased;

    private GameObject owner;
    
    
    
    private void Awake()
    {
        wasSelected = false;
        wasReleased = false;
        BoxCollider col = gameObject.AddComponent<BoxCollider>();  //using box cause it makes us not need to change code elsewhere
        col.isTrigger = true;
    }

    //This should be set when the game object is created
    public void Initialize(GameObject owner)
    {
        this.owner = owner;
    }

    public GameObject GetOwner()
    {
        return owner;
    }
    
    public void Select(GameObject controller)
    {
    //    Debug.Log("Selected obj:"+gameObject.name);

    if (!continueSpawning)
        return;

    if (wasSelected)  //so you can't catch a "grenade" and throw it back.
        return;
    
    Transform form = gameObject.transform;
    form.parent = controller.transform;
    form.position = controller.transform.position;
    continueSpawning = false;
    wasSelected = true;

    }
    

    public void OnTriggerRelease(GameObject controller)
    {
        if (!wasSelected)
            return;

        //on trigger released is called multiple times in different contexts so 
        //we wanna ensure we're not redoing stuff for a potion already thrown
        if (wasReleased)
            return;
        

        wasReleased = true;
        gameObject.transform.parent = null;
        continueSpawning = true;
        
        PrepareFlight(controller);
    }


    //basically get the thing ready for flying
    //the idea is that if you call this method, then the object starts to fly according to it's thrown 
    //trajectory (no homing)
    //the update function will eventually call another method to make it a homing missile.
    //this is for the player.
    public void PrepareFlight(GameObject controller)
    {
       BoxCollider col = gameObject.GetComponent<BoxCollider>();
       col.isTrigger = false;

       Rigidbody body = gameObject.AddComponent<Rigidbody>();
       SteamVR_Behaviour_Pose pos = controller.GetComponent<SteamVR_Behaviour_Pose>();
       
       
       Vector3 vel = pos.GetVelocity(); //this is LOCAL velocity
       
       float mag = vel.magnitude;
       vel = vel.normalized;
       
       float[] rotations = Rotate(vel.x, vel.z); //rotate by 90* to correct the direction --if the camera had a 
                                                     //higher heirarchy we'd need to do more work
        Vector3 res = new Vector3(rotations[0], vel.y, rotations[1]);
        res *= mag;
           //transform.transformPoint() did not work on this for some reason...
        
        body.velocity = res;
        
    }
    

    private float[] Rotate(float x, float y)
    {
        const float ANGLE_RADS = 1.57f;  //this is 90 degrees
        
        double nx = x*Math.Cos(ANGLE_RADS) - y*Math.Sin(ANGLE_RADS);
        double ny = x*Math.Sin(ANGLE_RADS) + y*Math.Cos(ANGLE_RADS);
        return new float[] { (float)nx, (float)ny };
    }
    
    
    
    private static string[] tags = new string[] { "Crate","Environment" };
    
//    !!!Untested!!! (all below)
    public void OnTriggerEnter(Collider other)
    {
        if (!wasReleased)
        {
    
            return;
        }

        GameObject hitObject = other.gameObject;

        foreach (string s in tags)
            if (hitObject.tag.Equals(s))
            {
                Explode();
                return;
            }

        Entity e = hitObject.GetComponent<Entity>();
        if (e != null)
        {
   
          bool damaged = e.damageEntity(gameObject);
          Debug.Log("damage:"+damaged);
          if (damaged)
              Explode();

          return;
        }
    }


    public void Explode()
    {
        //do some cool fun particle stuff here
        Destroy(gameObject);
        
    }
}
