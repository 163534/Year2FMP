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
    Vector3 point;
    RaycastHit hit;
    Ray ray;
    Vector3 direction;
    int playerint;
    int fireballint;
    LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
    
        transform.parent = null;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        fsm = player.GetComponent<PlayerFSM>();
        _targetTransform = fsm.targetTransform;

        playerint = 1 << LayerMask.NameToLayer("Player");
        fireballint = 1 << LayerMask.NameToLayer("Fireball");
        mask = playerint | fireballint;
        Debug.Log(mask.value);

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        projectileSpeed = 10;

        if (Physics.Raycast(ray, out hit, mask))
        {
            _targetTransform = hit.point;
            Vector3 direction = (_targetTransform - transform.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        /*point = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        transform.LookAt(point);*/
        //rb.velocity = transform.forward * projectileSpeed;// direction * projectileSpeed;
    }

    // Update is called once per frame
}
