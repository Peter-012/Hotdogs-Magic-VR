using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WandLogic : MonoBehaviour {
    private SteamVR_Input_Sources InputSource;
    private SteamVR_Action_Boolean ActionBoolean;

    [SerializeField] private string ProjectilePath = "Projectile";

    private bool reloading = false;
    private float reloadDelay = 2f;
    
    private void Start() {
        initSteamVR();
    }

    private void Update() {
        if (ActionBoolean.GetStateDown(InputSource) && reloading == false) {
            fireProjectile();
            StartCoroutine(Reload());
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
        Object projectilePrefab = Resources.Load<Object>(ProjectilePath);
        GameObject projectileObject = Instantiate(projectilePrefab, position, rotation) as GameObject;
        projectileObject.name = "ProjectilePlayer";

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
