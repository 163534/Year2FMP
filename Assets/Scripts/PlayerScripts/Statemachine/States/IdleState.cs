using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    // WARNING! This state has effectively turned into a checker state \\

    PlayerFSM sm;
    public void OnEnterState(PlayerFSM stateMachine)
    {
        sm = stateMachine;
        Debug.Log("Entered Idle State");
        sm.InitDebugText();
    }
    public void UpdateState()
    {
        if (Aiming())
        {
            sm.ChangeState(sm.aimState);
        }
    }
    public void PhysicsUpdateState()
    {

    }
    public void OnExitState()
    {

    }
    public bool Aiming()
    {
        if (Input.GetMouseButtonDown(0))
            return true;
        else
            return false;
    }
}
