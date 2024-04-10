using UnityEngine;

public class EnemyStateIdle : EnemyStateAbstract {
    [SerializeField] private float idleTime = 2f;
    private float currentTime;

    public override void EnterState
    (EnemyStateManager state) {
        currentTime = 0;
    }

    public override void UpdateState
    (EnemyStateManager state) {
        if (currentTime >= idleTime) state.ChangeState(state.Shoot);
        else currentTime += Time.deltaTime;
    }
}
