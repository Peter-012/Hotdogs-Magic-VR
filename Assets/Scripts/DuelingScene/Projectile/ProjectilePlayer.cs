using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : ProjectileAbstract {
    [SerializeField] private float projectileSpeed = 3f;
    [SerializeField] private float delayFire = 1f;
    [SerializeField] private float deleteProjectile = 3f;
    private bool charging = false;

    [SerializeField] private Vector3 gameObjectSize = new Vector3(0.25f, 0.25f, 0.25f);
    private Vector3 initialPos;
    private BoxCollider boundingBox;
    
    private Object effect;
    
    private void Start() {
        boundingBox = gameObject.AddComponent<BoxCollider>();
        effect = Resources.Load<Object>("ParticleExplosionBlue");
        StartCoroutine(Charging());
    }

    public override void fireProjectile() {
        // Charging - Keep the projectile stuck to the wand
        if (charging) return;

        // Detach projectile from wand
        this.transform.SetParent(null);

        // Let the projectile travel forward
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

        // !charging prevent player from accidentally destroying projectile 
        // during charge state when hitting it against the floor
        if (!charging && hitObject.tag.Equals("Environment")) {
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

    IEnumerator Charging() {
        charging = true;
        yield return new WaitForSeconds(delayFire);
        charging = false;
    }
}
