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
        sm.MovementCalculationAndCamera();
        sm.Jump();
        sm.AimDownSights();
    }
    public void PhysicsUpdateState()
    {

    }
    public void OnExitState()
    {

    }
    
    
}
