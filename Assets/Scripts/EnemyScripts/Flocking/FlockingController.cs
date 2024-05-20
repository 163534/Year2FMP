using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface EState
{
    public void UpdateState();
    public void PhysicsUpdateState();
    public void OnEnterState(FlockingController sm);
    public void OnExitState();
}
public class FlockingController : MonoBehaviour
{
    public EState currentState, lastState;

    //public Vector3 bound;
    public float speed;
    public float distanceFromTarget;

    //private Vector3 initialPosition;
    private Vector3 nextMovementPoint;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //initialPosition = transform.position;

        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        StayOffTheGround();
        ChasePlayer();
        if (currentState != null)
        {
            currentState.UpdateState();
        }
        /* if(Vector3.Distance(nextMovementPoint, transform.position) <= targetReachedRadius)
         {
             CalculateNextMovementPoint();
         }*/
    }
    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsUpdateState();
        }
    }
    public void ChangeState(EState newState)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        lastState = currentState;
        currentState = newState;
        currentState.OnEnterState(this);
    }
    void ChasePlayer()
    {
        if (!WithinRangeOfPlayer())
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);
        }
    }
    void StayOffTheGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 5, Color.red);
        if(Physics.Raycast(transform.position, Vector3.down, 5))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
    void FindPlayer()
    {
       /* float posX = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
        float posY = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
        float posZ = Random.Range(initialPosition.x - bound.z, initialPosition.z + bound.z); */

        nextMovementPoint = player.transform.position;
    }
    // SS into weekly write-ups.
    bool WithinRangeOfPlayer()
    {
        Debug.Log(Vector3.Distance(gameObject.transform.position, player.transform.position));
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) <= distanceFromTarget)
        {
            return true;
        }
        return false;
    }
}
