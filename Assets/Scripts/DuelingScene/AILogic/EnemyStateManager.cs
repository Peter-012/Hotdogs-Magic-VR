using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateAbstract {
    public abstract void EnterState
        (EnemyStateManager state);

    public abstract void UpdateState
        (EnemyStateManager state);

    public abstract void OnCollisionEnter
        (EnemyStateManager state, Collision collision);
}

public class EnemyStateManager : MonoBehaviour {
    EnemyStateAbstract currentState;

    // Possible Enemy States
    public EnemyStateIdle Idle = new EnemyStateIdle();

    void Start() {
        currentState = Idle;
        currentState.EnterState(this);
    }

    void Update() {
        currentState.UpdateState(this);
    }

    void OnCollisionEnter(Collision collision) {
        currentState.OnCollisionEnter(this, collision);
    }

    public void ChangeState(EnemyStateAbstract state) {
        currentState = state;
        state.EnterState(this);
    }
}
