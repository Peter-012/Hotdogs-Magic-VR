using UnityEngine;

public class EnemyStateIdle : EnemyStateAbstract {
    public override void EnterState
    (EnemyStateManager state) {
        Debug.Log("Works");
    }

    public override void UpdateState
    (EnemyStateManager state) {
        // if () {}
        // else {
        //     state.ChangeState(state.ShootState);
        // }
    }

    public override void OnCollisionEnter
    (EnemyStateManager state, Collision collision) {

    }
}
