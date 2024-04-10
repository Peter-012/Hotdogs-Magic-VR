using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Player.name.Equals("Camera")) {
            Player1.health--;
            if (Player1.health <= 0) {
                //make steamvr view as grey
            }else if (Player1.health <= 3) {
                //make steamvr view as red
            }
            base.DestroyProjectile();
        }

        // Logic for projectile collision to environment (From abstract class)
        base.ProjectileHit(Player);
    }
}
