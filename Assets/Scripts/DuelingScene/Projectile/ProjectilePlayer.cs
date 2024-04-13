using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : ProjectileAbstract {
    [SerializeField] private float projectileSpeed = 3f;
    [SerializeField] private float delayFire = 1f;
    [SerializeField] private float deleteProjectile = 3f;
    [SerializeField] private float enemyFaceplantForce = 2f;
    private bool charging = false;
    
    private void Start() {
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

    IEnumerator Charging() {
        charging = true;
        yield return new WaitForSeconds(delayFire);
        charging = false;
    }

    IEnumerator Travel() {
        float currentTime = 0;

        while (currentTime < deleteProjectile) {
            // Update elapsed time
            currentTime += Time.deltaTime;

            // Move projectile forward
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

            yield return null;
        }

        // Projectile expires
        Destroy(gameObject);
    }

    public override void ProjectileHit (GameObject Player) {
        if (Player.name.Equals("Enemy")) {
            Player2.health--;
            if (Player2.health <= 0 && Game.startGame == true) {
                Game.startGame = false;
                KillEnemy(Player);
                FadePlayer();
            } 
            base.DestroyProjectile();
        }
    }

    private void KillEnemy(GameObject enemy) {
        // Disable projectile damage
        BoxCollider enemyCollider = GameObject.Find("Enemy").GetComponent<BoxCollider>();
        enemyCollider.isTrigger = false;

        BoxCollider playerCollider = GameObject.Find("Camera").GetComponent<BoxCollider>();
        playerCollider.isTrigger = false;

        // Make the enemy fall down
        Rigidbody enemyRigid = enemy.GetComponent<Rigidbody>();
        enemyRigid.useGravity = true;
        enemyRigid.isKinematic = false;
        enemyRigid.AddForce(Vector3.right * enemyFaceplantForce, ForceMode.Impulse);

        // Fade back to main menu
        BoxCollider boxCollider = GameObject.Find("Camera").GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    private void FadePlayer() {
        // Fade out back to main menu
        TransistionScene transition = GameObject.Find("[CameraRig]").GetComponent<TransistionScene>();
        transition.fadeOutToScene(3f, "MenuScene");
    }
}
