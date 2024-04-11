using UnityEngine;

public class EnemyStateShoot : EnemyStateAbstract {
    [SerializeField] private float delayShot = 1f;
    private float currentTime;

    [SerializeField] private string ProjectilePath = "Projectile";

    public override void EnterState
    (EnemyStateManager state) {
        fireProjectile();
        currentTime = 0;
    }

    public override void UpdateState
    (EnemyStateManager state) {
        if (currentTime >= delayShot) state.ChangeState(state.Idle);
        else currentTime += Time.deltaTime; // Stand still for a while to charge projectile
    }

    private void fireProjectile() {
        GameObject enemyObject = GameObject.Find("Enemy");
        Vector3 position = enemyObject.transform.position + enemyObject.transform.up;
        Quaternion rotation = enemyObject.transform.rotation;

        // Spawn a projectile from the enemy
        Object projectilePrefab = Resources.Load<Object>(ProjectilePath);
        GameObject projectileObject = GameObject.Instantiate(projectilePrefab, position, rotation) as GameObject;

        // Attach projectile to the enemy
        projectileObject.transform.SetParent(GameObject.Find("Enemy").transform);

        // Make the projectile rigid
        Rigidbody projectileRigid = projectileObject.AddComponent<Rigidbody>();
        projectileRigid.useGravity = false;
        projectileRigid.isKinematic = true;

        // Make the projectile have a box collider
        BoxCollider boxCollider = projectileObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // Add component for projectile logic
        projectileObject.AddComponent<ProjectileEnemy>();
    }
}
