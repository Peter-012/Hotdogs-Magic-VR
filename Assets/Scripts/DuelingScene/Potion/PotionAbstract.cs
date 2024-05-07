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
    private Vector3 initialPos;
    
    public static event Action <GameObject> OnCollision;
    private BoxCollider boundingBox;
    // private Object effect;

    // public abstract void playExplosion();

    public virtual void Start() {
        // effect = Resources.Load<Object>("ParticleExplosionRed");

        boundingBox = gameObject.GetComponent<BoxCollider>();
        if (boundingBox == null)
            boundingBox = gameObject.AddComponent<BoxCollider>();

        boundingBox.isTrigger = true;

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

            // Keep track of the previous position of potion
            initialPos = transform.position;

            // Move potion forward
            transform.Translate(Vector3.forward * potionSpeed * Time.deltaTime);

            updateBoundingBox();

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
        // playExplosion();
        Destroy(gameObject);
    }

    // public override void playExplosion() {
    //     StartCoroutine(particleTimer());
    // }

    // private IEnumerator particleTimer() {
    //     GameObject particles = 
    //         Instantiate(effect, gameObject.transform.position, Quaternion.identity) as GameObject;
    //     yield return new WaitForSeconds(1f);
    //     Destroy(particles);
    // }

    private void updateBoundingBox() {
        // Final position will always be at local (0, 0, 0)

        // Initial Position Relative to Potion Final Position
        initialPos = transform.InverseTransformPoint(initialPos);

        // Box Collider Center //

        // Since final = 0, center = (initial + final) / 2

        boundingBox.center = initialPos * 0.5f;

        // Box Collider Size //

        // Distance between initial and final position

        // Since final = 0, distance = Sqrt(Pow(initialPos.z, 2) + Pow(initialPos.z, 2))
        // distance = Sqrt(Pow(initialPos.z, 2))

        float distanceZ = Mathf.Sqrt(Mathf.Pow(initialPos.z, 2));

        // This could be simplified to (distanceZ = initialPos.z) 
        // but it seems to make the size of the bounding box smaller than normal

        // boxSize = initialDefaultSize/2 + finalDefaultSize/2 + distanceZ
        // Since initialDefaultSize == finalDefaultSize == (gameObjectSize)
        // boxSize = gameObjectSize + distanceZ

        Vector3 boxSize = gameObjectSize;
        boxSize.z += distanceZ;

        boundingBox.size = boxSize;
    }

    public abstract void AimPotion();
}
