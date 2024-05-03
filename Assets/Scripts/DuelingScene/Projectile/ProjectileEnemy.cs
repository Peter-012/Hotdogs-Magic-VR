using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ProjectileEnemy : ProjectileAbstract {
    [SerializeField] private float horizontalNoise = 4f;
    [SerializeField] private float verticalNoise = 2f;

    [SerializeField] private float projectileSpeed = 0.3f;
    [SerializeField] private float deleteProjectile = 3f;

    [SerializeField] private Vector3 gameObjectSize = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 initialPos;
    private BoxCollider boundingBox;

    private Object effect;

    void Start() {
        boundingBox = gameObject.AddComponent<BoxCollider>();
        boundingBox.isTrigger = true;
        AimProjectile();
        effect = Resources.Load<Object>("ParticleExplosionRed");
    }

    public override void fireProjectile() {
        StartCoroutine(Travel());
    }

    IEnumerator Travel() {
        float currentTime = 0;

        while (currentTime < deleteProjectile) {
            currentTime += Time.deltaTime;

            // Keep track of the previous position of projectile
            initialPos = transform.position;

            // Move projectile forward
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

            updateBoundingBox();

            yield return null;
        }

        // Projectile expires
        Destroy(gameObject);
    }

    private void updateBoundingBox() {
        // Final position will always be at local (0, 0, 0)

        // Initial Position Relative to Projectile Final Position
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

    public override void ProjectileHit(GameObject hitObject) {
        base.ProjectileHit(hitObject);
        if (hitObject.tag.Equals("Environment")) {
           DestroyProjectile();
       }
    }

    public override void playExplosion() {
        StartCoroutine(particleTimer());
    }

    private IEnumerator particleTimer() {
        GameObject particles = Instantiate(effect, gameObject.transform.position, Quaternion.identity) as GameObject;
        yield return new WaitForSeconds(1f);
        Destroy(particles);
        
    }
    
    private void AimProjectile() {
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
        float PlayerToProjectile = 
            Mathf.Abs(playerCenter - gameObject.transform.position.y);

        // Adjacent is already calculated from the horizontal angle calculation

        // Hypotenuse
        float enemyToPlayerVertical = 
            Mathf.Sqrt(Mathf.Pow(enemyToPlayerX, 2f) + Mathf.Pow(PlayerToProjectile, 2f));
        
        float verticalAngle = Mathf.Rad2Deg * Mathf.Asin(PlayerToProjectile/enemyToPlayerVertical);

        // Negative value when player center is above line of enemy fire
        if (playerCenter - gameObject.transform.position.y > 0) 
            verticalAngle = -verticalAngle;


        // Add noise to the angle data so that enemy is not 100% accurate
        float noiseHorizontal = Random.Range(0, horizontalNoise);
        if (Random.value < 0.5) noiseHorizontal = -noiseHorizontal; // Random horizontal angle (+/-)

        float noiseVertical = Random.Range(0, verticalNoise);
        if (Random.value < 0.5) noiseVertical = -noiseVertical; // Random vertical angle (+/-)

        horizontalAngle = horizontalAngle + noiseHorizontal;
        verticalAngle = verticalAngle + noiseVertical;

        // Adjust the trajectory of projectile
        gameObject.transform.Rotate(verticalAngle, horizontalAngle, 0);
    }
}
