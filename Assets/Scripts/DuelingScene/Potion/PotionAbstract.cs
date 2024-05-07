using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DuelingScene.Entity;

public abstract class PotionAbstract : MonoBehaviour, IDamage {
    [SerializeField] private float potionSpeed = 0.02f;
    [SerializeField] private float deletePotion = 30f;
    [SerializeField] protected float delayTime = 0.5f;

    protected bool delayPotionBool;

    [SerializeField] private Vector3 gameObjectSize = new Vector3(0.003f, 0.003f, 0.003f);
    
    public static event Action <GameObject> OnCollision;
    private BoxCollider potionCollider;

    public virtual void Start() {

        potionCollider = gameObject.GetComponent<BoxCollider>();
        if (potionCollider == null)
            potionCollider = gameObject.AddComponent<BoxCollider>();

        potionCollider.size = gameObjectSize;
        potionCollider.isTrigger = true;

        delayPotionBool = true;
        StartCoroutine(DelayPotion());
    }

    private void Update() {
        if (delayPotionBool) return;
        StartCoroutine(FirePotion());
    }

    public void Hit(GameObject Player) {
        if (OnCollision == null) return;
        OnCollision.Invoke(Player);
    }

    private void OnEnable() {
        OnCollision += PotionHit;
    }

    private void OnDisable() {
        OnCollision -= PotionHit;
    }

    private IEnumerator FirePotion() {
        float currentTime = 0;

        while (currentTime < deletePotion) {
            currentTime += Time.deltaTime;

            // Move potion forward
            transform.Translate(Vector3.forward * potionSpeed * Time.deltaTime);

            yield return null;
        }

        // Potion expires
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        PotionHit(other.gameObject);
    }

    public abstract IEnumerator DelayPotion();

    public void PotionHit(GameObject hitObject) {
        bool damage = false;

        Entity e = hitObject.GetComponent<Entity>();
        if (e != null) damage = e.damageEntity(gameObject);
       
        if (damage) DestroyPotion();
        if (
            hitObject.name.Equals("ProjectilePlayer") || 
            hitObject.name.Equals("ProjectileEnemy")
        ) DestroyPotion();
        if (hitObject.tag.Equals("Crate")) DestroyPotion();
        if (hitObject.tag.Equals("Environment")) DestroyPotion();
    }
    
    public void DestroyPotion() {
        Destroy(gameObject);
    }

    public abstract void AimPotion();
}
