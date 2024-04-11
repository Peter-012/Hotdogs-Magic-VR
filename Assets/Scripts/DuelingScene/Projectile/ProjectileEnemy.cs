using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ProjectileEnemy : ProjectileAbstract {
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float deleteProjectile = 4f;
    private float currentTime = 0;

    public override void fireProjectile() {
        // Destroy projectile after a certain amount of time
        if (currentTime > deleteProjectile) Destroy(gameObject);
        currentTime += Time.deltaTime;

        // Move projectile forward
        transform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
    }

    public override void ProjectileHit (GameObject Player) {
        // Logic for projectile collision to environment (From abstract class)
        base.ProjectileHit(Player);
        
        if (Player.name.Equals("Camera")) {
            Player1.health--;
            if (Player1.health <= 0) KillPlayer();
            base.DestroyProjectile();
        }
    }

    private void KillPlayer() {
        // Fade out back to main menu
        BoxCollider boxCollider = GameObject.Find("Camera").GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        TransistionScene transition = GameObject.Find("[CameraRig]").GetComponent<TransistionScene>();
        transition.fadeOutToScene(3f, "MenuScene");
    }

    // IEnumerator tintView(Color color, float fadeOutDuration, float alphaStart, float alphaEnd) {
    //     // Set initial time with fade color and alpha
    //     float currentTime = 0;
    //     Color currentColor = color;
    //     currentColor.a = alphaStart;

    //     // Gradually fade in/out player view
    //     while (currentTime < fadeOutDuration) {
    //         // Change alpha of color for tint view
    //         currentColor.a = Mathf.Lerp(
    //                 alphaStart, alphaEnd, currentTime/fadeOutDuration
    //         );
    //         SteamVR_Fade.View(currentColor, 0);

    //         // Update elapsed time
    //         currentTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     // Make sure that the tint alpha is consistently ending on alphaEnd
    //     currentColor.a = alphaEnd;
    //     SteamVR_Fade.View(currentColor, 0);
    // }
}
