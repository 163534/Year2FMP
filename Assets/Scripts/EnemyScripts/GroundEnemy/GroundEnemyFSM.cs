using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ChestState
{
    Chase,
    Attack,
}
public class GroundEnemyFSM : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    GameObject player;
    ChestState state;
    public float distanceFromTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        state = ChestState.Chase;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == ChestState.Chase)
        {
            FindPlayer();
        }
        if(state == ChestState.Attack)
        {
            LookAtPlayer();
            AttackPlayer();
        }
    }
    void FindPlayer()
    {
        agent.SetDestination(player.transform.position);
        anim.SetBool("Attack", false);

        if (WithinRangeOfPlayer())
        {
            state = ChestState.Attack;
        }

    }
    void AttackPlayer()
    {
        anim.SetBool("Attack", true);

        if (!WithinRangeOfPlayer())
        {
            state = ChestState.Chase;
        }
    }
    void LookAtPlayer()
    {
        var rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2.5f);
    }
    bool WithinRangeOfPlayer()
    {
        //Debug.Log(Vector3.Distance(gameObject.transform.position, player.transform.position));
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= distanceFromTarget)
        {
            return true;
        }
        return false;
    }
}
