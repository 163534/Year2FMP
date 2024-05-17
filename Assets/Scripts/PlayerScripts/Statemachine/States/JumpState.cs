using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{

    PlayerFSM sm;
    public void OnEnterState(PlayerFSM stateMachine)
    {
        sm = stateMachine;

        Debug.Log("Entered Jump State");
        sm.InitDebugText();
    }
    public void UpdateState()
    {

      //  sm.Jump();
      //  sm.AimDownSights();
    }
    public void PhysicsUpdateState()
    {

    }
    public void OnExitState()
    {

    }
}
