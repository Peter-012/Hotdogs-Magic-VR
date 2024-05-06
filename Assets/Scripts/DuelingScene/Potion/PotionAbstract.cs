using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DuelingScene.Entity;

public abstract class PotionAbstract : MonoBehaviour, IDamage {
    [SerializeField] private float potionSpeed = 0.01f;
    [SerializeField] private float deletePotion = 3f;
    [SerializeField] private float delayTime = 0.5f;

    private bool delayPotion;

    [SerializeField] private Vector3 gameObjectSize = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 initialPos;
    
    public static event Action <GameObject> OnCollision;
    private BoxCollider boundingBox;
    // private Object effect;

    // public abstract void playExplosion();

    void Start() {
        // effect = Resources.Load<Object>("ParticleExplosionRed");

        boundingBox = gameObject.AddComponent<BoxCollider>();
        boundingBox.isTrigger = true;

        delayPotion = true;
        StartCoroutine(DelayPotion());
    }

    private void Update() {
        if (delayPotion) return;
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

    private IEnumerator DelayPotion() {
        yield return new WaitForSeconds(delayTime);

        Rigidbody potionRigid = 
            gameObject.GetComponent<Rigidbody>();
        potionRigid.useGravity = false;
        potionRigid.isKinematic = true;

        // Reset orientation of the potion
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, 90, 0);

        AimPotion();

        delayPotion = false;
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

    public virtual void PotionHit(GameObject hitObject) {
        bool damage = false;

        Entity e = hitObject.GetComponent<Entity>();
        if (e != null) damage = e.damageEntity(gameObject);
       
        if (damage) DestroyPotion();
        if (hitObject.tag.Equals("Crate")) DestroyPotion();
        if (hitObject.tag.Equals("Environment")) DestroyPotion();
    }
    
    public virtual void DestroyPotion() {
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

    private void AimPotion() {
        GameObject playerObject = GameObject.Find("Camera");
        GameObject rigObject = GameObject.Find("[CameraRig]");

        //// Calculating Horizontal Angle ////

        // Pythagorean Theorem
        
        // Opposite
        float enemyToPlayerZ = 
            Mathf.Abs(gameObject.transform.position.z - playerObject.transform.position.z);

        // Adjacent
        float enemyToPlayerX = 
            Mathf.Abs(playerObject.transform.position.x - gameObject.transform.position.x);

        // Hypotenuse
        float enemyToPlayerHorizontal = 
            Mathf.Sqrt(Mathf.Pow(enemyToPlayerZ, 2f) + Mathf.Pow(enemyToPlayerX, 2f));

        float horizontalAngle = Mathf.Rad2Deg * Mathf.Asin(enemyToPlayerZ/enemyToPlayerHorizontal);

        // Negative depending on if enemy is past player Z position
        if (playerObject.transform.position.z - gameObject.transform.position.z > 0) 
            horizontalAngle = -horizontalAngle;


        //// Calculating Vertical Angle ////

        // Pythagorean Theorem

        // Adjust to aim a bit above the player's center of mass
        float playerCenter = 
            Mathf.Abs(playerObject.transform.position.y - rigObject.transform.position.y) * 0.66f;

        // Add back the camera rig offset
        playerCenter = playerCenter + rigObject.transform.position.y;

        // Opposite
        float PlayerToPotion = 
            Mathf.Abs(playerCenter - gameObject.transform.position.y);

        // Adjacent is already calculated from the horizontal angle calculation

        // Hypotenuse
        float enemyToPlayerVertical = 
            Mathf.Sqrt(Mathf.Pow(enemyToPlayerX, 2f) + Mathf.Pow(PlayerToPotion, 2f));
        
        float verticalAngle = Mathf.Rad2Deg * Mathf.Asin(PlayerToPotion/enemyToPlayerVertical);

        // Negative value when player center is above line of enemy fire
        if (playerCenter - gameObject.transform.position.y > 0) 
            verticalAngle = -verticalAngle;

        // Adjust the trajectory of potion
        gameObject.transform.Rotate(verticalAngle, horizontalAngle, 0);
    }
}
