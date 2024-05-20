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
        if( sm.CheckForMove() )
        {
            sm.ChangeState(sm.runState);
        }
        sm.MovementCalculationAndCamera();
        sm.AimDownSights();
    }
    public void PhysicsUpdateState()
    {

    }
    public void OnExitState()
    {

    }
    
    
}
