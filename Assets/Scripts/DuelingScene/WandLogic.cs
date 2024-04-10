using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WandLogic : MonoBehaviour {
    private SteamVR_Input_Sources InputSource;
    private SteamVR_Action_Boolean ActionBoolean;

    [SerializeField] private string ProjectilePath = "Projectile";
    private Object projectilePrefab;

    private bool reloading = false;
    private float reloadDelay = 1f;
    
    private void Start() {
        initSteamVR();

        // Load Projectile Prefab
        projectilePrefab = Resources.Load<Object>(ProjectilePath);
    }

    private void Update() {
        if (ActionBoolean.GetStateDown(InputSource)) {
            if (reloading == false) {
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

    private void initSteamVR() {
        // Initialize InputSource

        if (Player1.DominantSide.Equals("left")) {
            InputSource = SteamVR_Input_Sources.LeftHand;
        }
        else if (Player1.DominantSide.Equals("right")) {
            InputSource = SteamVR_Input_Sources.RightHand;
        }
        else {
            Debug.LogError("Failed to initialize SteamVR input source.");
        }

        // Initialize ActionBoolean
        ActionBoolean = SteamVR_Input
            .GetActionFromPath<SteamVR_Action_Boolean>("/actions/default/in/GrabPinch");
    }

    private void fireProjectile() {
        Vector3 position = this.transform.position + this.transform.forward;
        Quaternion rotation = this.transform.rotation;

        // Spawn a projectile from the wand
        GameObject projectileObject = Instantiate(projectilePrefab, position, rotation) as GameObject;
        projectileObject.name = "Projectile";

        // Rigidbody projectileRigid = projectileObject.AddComponent<Rigidbody>();
        // projectileRigid.useGravity = false;
        // projectileRigid.isKinematic = true;

        // Attach projectile to the wand
        projectileObject.transform.SetParent(GameObject.Find("Wand").transform);

        // Move projectile to the tip of the wand
        projectileObject.transform.Translate(0, 0, -0.55f);

        // Add component for projectile logic
        projectileObject.AddComponent<ProjectileLogic>();
    }
}
