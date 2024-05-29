using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum boidControllerState
{
    Chase,
    Attack,
}
public class FlockingController : MonoBehaviour
{
    
    //public Vector3 bound;
    public float speed;
    public float distanceFromTarget;
    public bool attack;

    //private Vector3 initialPosition;
    private Vector3 nextMovementPoint;

    private GameObject player;
    public boidControllerState state;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //initialPosition = transform.position;
        state = boidControllerState.Chase;
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        print(state);
        if(state == boidControllerState.Chase)
        {
            CheckForStateChange();
            FindPlayer();
            StayOffTheGround();
            ChasePlayer();
        }
        if(state == boidControllerState.Attack)
        {
            CheckForStateChange();
        }
          /* if(Vector3.Distance(nextMovementPoint, transform.position) <= targetReachedRadius)
         {
             CalculateNextMovementPoint();
         }*/
    }

    void ChasePlayer()
    {
        if (!WithinRangeOfPlayer())
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);
        }

    }
    void CheckForStateChange()
    {
        if (!WithinRangeOfPlayer())
        {
            state = boidControllerState.Chase;
        }
        else if (WithinRangeOfPlayer())
        {
            state = boidControllerState.Attack;
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
        //Debug.Log(Vector3.Distance(gameObject.transform.position, player.transform.position));
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) <= distanceFromTarget)
        {
            return true;
        }
        return false;
    }
}
