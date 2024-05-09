using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : IState
{
    PlayerFSM sm;
    GameObject mainCamera;
    GameObject aimCamera;


    public void OnEnterState(PlayerFSM stateMachine)
    {
        sm = stateMachine;
        sm.InitDebugText();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        aimCamera = GameObject.FindGameObjectWithTag("AimCamera");
        Debug.Log("Entered Aim state");
    }
    public void UpdateState()
    {
        Debug.Log("Idle =" + sm.idleState);
       /* if (!idleS.Aiming())
        {
            sm.ChangeState(sm.idleState);
        }*/
    }
    public void PhysicsUpdateState()
    {

    }
    public void OnExitState()
    {

    }

}
