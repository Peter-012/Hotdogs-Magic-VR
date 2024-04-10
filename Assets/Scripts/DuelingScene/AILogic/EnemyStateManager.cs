using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateAbstract {
    public abstract void EnterState
        (EnemyStateManager state);

    public abstract void UpdateState
        (EnemyStateManager state);
}

public class EnemyStateManager : MonoBehaviour {
    EnemyStateAbstract currentState;

    // Possible Enemy States
    public EnemyStateIdle Idle = new EnemyStateIdle();
    public EnemyStateShoot Shoot = new EnemyStateShoot();

    void Start() {
        currentState = Idle;
        currentState.EnterState(this);
    }

    void Update() {
        currentState.UpdateState(this);
    }

    public void ChangeState(EnemyStateAbstract state) {
        currentState = state;
        state.EnterState(this);
    }
}
