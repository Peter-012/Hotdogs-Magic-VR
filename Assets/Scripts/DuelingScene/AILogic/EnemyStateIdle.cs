using UnityEngine;

public class EnemyStateIdle : EnemyStateAbstract {
    [SerializeField] private float movementRange = 5.0f;
    [SerializeField] private float enemyCenter = 0.45f;
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private float speed = 1.0f;
    private float currentZPos;
    private float finalZPos;

    private GameObject enemyObject;

    public override void EnterState
    (EnemyStateManager state) {
        // Store current position of the enemy
        enemyObject = GameObject.Find("Enemy");
        currentZPos = enemyObject.transform.position.z;

        // Pick a random location for the enemy to go to
        float rightMax = movementRange/2 + enemyCenter;
        float leftMax = -rightMax;
        finalZPos = Random.Range(leftMax, rightMax);
    }

    public override void UpdateState
    (EnemyStateManager state) {
        // Update position of the enemy
        currentZPos = enemyObject.transform.position.z;
        float offset = Mathf.Abs(currentZPos - finalZPos);
        // Debug.Log(" Curr: " + currentZPos + " Final: " + finalZPos + " Offset: " + offset);

        if (offset <= threshold) {
            // Change enemy to shoot state when it has moved to the target position
            // Meaning that the enemy is within threshold
            state.ChangeState(state.Shoot);
        } else {
            // Move enemy to the random position
            if (currentZPos - finalZPos > 0)
                enemyObject.transform.Translate(-Vector3.right * speed * Time.deltaTime);
            else
                enemyObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
