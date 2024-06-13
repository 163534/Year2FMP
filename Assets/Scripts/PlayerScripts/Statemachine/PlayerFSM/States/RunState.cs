using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    // WARNING! This state has effectively turned into a checker state \\

    PlayerFSM sm;
    
    public void OnEnterState(PlayerFSM stateMachine)
    {
        sm = stateMachine;

        //Debug.Log("Entered Run State");
        sm.InitDebugText();
    }
    public void UpdateState()
    {

        if (sm.CheckForMove() == false)
        {
            sm.ChangeState(sm.idleState);
        }
        else if(sm.CheckForMove() == true)
        {
            sm.ApplyGravity();
            sm.MovementCalculationAndCamera();
        }
        sm.AnimateMovement();
        sm.AimDownSights();
        sm.ShootFireBall();
        sm.Jump();
        sm.MoveCC();



        //sm.AimDownSights();
    }
    public void PhysicsUpdateState()
    {
        //sm.MoveCC();
    }
    public void OnExitState()
    {

    }


}





