using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DuelingScene.Entity;

public abstract class ProjectileAbstract : MonoBehaviour, IDamage {
    public static event Action <GameObject> OnCollision;

    public abstract void playExplosion();
    

    private void Update() {
        fireProjectile();
    }

    public void Hit(GameObject Player) {
        if (OnCollision == null) return;
        OnCollision.Invoke(Player);
    }

    private void OnEnable() {
        OnCollision += ProjectileHit;
    }

    private void OnDisable() {
        OnCollision -= ProjectileHit;
    }

    public abstract void fireProjectile();

    
    
    private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log("projectile hit:"+other.gameObject.name+" from: "+gameObject.GetType().FullName);
        ProjectileHit(other.gameObject);
    }


    //called when projectile hit
    public void ProjectileHit(GameObject hitObject)
    {
       Entity e = hitObject.GetComponent<Entity>();
       bool damage = false;
       if (e != null)
          damage = e.damageEntity(gameObject);  //the current game object
       
       //do the trigger stuff here for the wand + flashy bits
       if (damage)
        DestroyProjectile();
       
       //now check if it is environmental

       string tag = hitObject.tag;
       if (tag.Equals("Environment"))
       {
           DestroyProjectile();
       }

       if (tag.Equals("Crate"))
       {
           ///do stuff here
           DestroyProjectile();
       }
       
    }
    
    
    public virtual void DestroyProjectile() {
        
        playExplosion();
        Destroy(gameObject);
    }
}
