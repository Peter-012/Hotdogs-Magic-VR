using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : ProjectileAbstract {
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float deleteProjectile = 4f;
    [SerializeField] private float delayFire = 1f;
    private float currentTime = 0;
    private bool detachFromWand = false;

    public override void fireProjectile() {
        // Detach projectile from wand
        if (currentTime >= delayFire) {
            if (detachFromWand) {
                if (currentTime > deleteProjectile) Destroy(gameObject);
                // Move projectile forward
                transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            } else {
                // Detach projectile from the wand
                this.transform.SetParent(null);
                detachFromWand = true;
            }
        }
        currentTime += Time.deltaTime;
    }

    public override void ProjectileHit (GameObject Player) {
        // Logic for projectile collision to environment (From abstract class)
        base.ProjectileHit(Player);

        if (Player.name.Equals("Enemy")) {
            Player2.health--;
            if (Player2.health <= 0 && GameManager.startGame == true) {
                GameManager.startGame = false;
                DestroyEnemy(Player);
                FadePlayer();
            } 
            base.DestroyProjectile();
        }
    }

    private void DestroyEnemy(GameObject enemy) {
        Destroy(enemy);

        // Fade back to main menu
        BoxCollider boxCollider = GameObject.Find("Camera").GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        TransistionScene transition = GameObject.Find("[CameraRig]").GetComponent<TransistionScene>();
        transition.fadeOutToScene(3f, "MenuScene");
    }

    private void FadePlayer() {
        // Disable projectile damage
        BoxCollider enemyCollider = GameObject.Find("Enemy").GetComponent<BoxCollider>();
        enemyCollider.isTrigger = false;

        BoxCollider playerCollider = GameObject.Find("Camera").GetComponent<BoxCollider>();
        playerCollider.isTrigger = false;

        // Fade out back to main menu
        TransistionScene transition = GameObject.Find("[CameraRig]").GetComponent<TransistionScene>();
        transition.fadeOutToScene(3f, "MenuScene");
    }
}
