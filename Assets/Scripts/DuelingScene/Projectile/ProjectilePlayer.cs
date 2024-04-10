using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : ProjectileAbstract {
    [SerializeField] private float delayFire = 1f;
    private float currentTime = 0;
    private bool detachFromWand = false;

    public override void fireProjectile() {
        // Detach projectile from wand
        if (currentTime >= delayFire) {
            if (detachFromWand) base.fireProjectile();

            // Detach projectile from the wand
            this.transform.SetParent(null);
            detachFromWand = true;
        }
        currentTime += Time.deltaTime;
    }

    public override void ProjectileHit (GameObject Player) {
        if (Player.name.Equals("Enemy")) {
            Player2.health--;
            if (Player2.health <= 0) DestroyEnemy(Player);
            base.DestroyProjectile();
        }

        // Logic for projectile collision to environment (From abstract class)
        base.ProjectileHit(Player);
    }

    private void DestroyEnemy(GameObject enemy) {
        Destroy(enemy);
    }
}
