using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float projectileSpeed;

    Vector3 _targetTransform;
    GameObject player;
    PlayerFSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        player = GameObject.FindWithTag("Player");
        fsm = player.GetComponent<PlayerFSM>();
        _targetTransform = fsm.targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveFireball();
    }
    private void MoveFireball()
    {
        Vector3 direction = (_targetTransform - transform.position).normalized;

        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

    }
}
