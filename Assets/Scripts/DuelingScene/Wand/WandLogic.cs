using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandLogic : MonoBehaviour {
    [SerializeField] private string ProjectilePath = "ProjectilePlayer";

    private float initialRotation = -0.5f;
    private float finalRotation = 0;
    [SerializeField] private float threshold = 0.2f;

    float initialZUpper;
    float initialZLower;
    float finalZUpper;
    float finalZLower;

    private bool trackRotation;
    private float currentZRotation;

    private bool reloading = false;
    [SerializeField] private float reloadDelay = 1.1f;
    
    private void Start() {
        // Set up logic to flick wand in order to fire projectile
        initialZUpper = initialRotation + threshold;
        initialZLower = initialRotation - threshold;
        finalZUpper = finalRotation + threshold;
        finalZLower = finalRotation - threshold;

        trackRotation = false;
    }

    private void Update() {
        currentZRotation = gameObject.transform.rotation.z;

        // Wand reached orientation to prepare for firing projectile
        if (initialZLower < currentZRotation && currentZRotation < initialZUpper) {
            if (!reloading) trackRotation = true;
        }
        
        if (trackRotation) {
            // Wand reached orientation to fire the projectile 
            if (finalZLower < currentZRotation && currentZRotation < finalZUpper) {
                trackRotation = false;
                fireProjectile();
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload() {
        reloading = true;
        yield return new WaitForSeconds(reloadDelay);
        reloading = false;
    }

    private void fireProjectile() {
        Vector3 position = this.transform.position + this.transform.forward;
        Quaternion rotation = this.transform.rotation;

        // Spawn a projectile from the wand
        Object projectilePrefab = Resources.Load<Object>(ProjectilePath);
        GameObject projectileObject = Instantiate(projectilePrefab, position, rotation) as GameObject;
        projectileObject.name = ProjectilePath;

        // Make the projectile rigid
        Rigidbody projectileRigid = projectileObject.AddComponent<Rigidbody>();
        projectileRigid.useGravity = false;
        projectileRigid.isKinematic = true;

        // Make the projectile have a box collider
        BoxCollider boxCollider = projectileObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.5f, 0.5f, 0.5f);
        boxCollider.isTrigger = true;

        // Attach projectile to the wand
        projectileObject.transform.SetParent(GameObject.Find("Wand").transform);

        // Move projectile to the tip of the wand
        projectileObject.transform.Translate(0, 0, -0.55f);

        // Add component for projectile player logic
        projectileObject.AddComponent<ProjectilePlayer>();

        // Add spatial audio
        projectileObject.AddComponent<SpatialAudio>();
    }
}
