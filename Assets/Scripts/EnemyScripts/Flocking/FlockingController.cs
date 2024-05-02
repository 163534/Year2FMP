using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour
{
    public Vector3 bound;
    public float speed = 1.0f;
    public float targetReachedRadius = 10.0f;

    private Vector3 initialPosition;
    private Vector3 nextMovementPoint;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        CalculateNextMovementPoint();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position) , 1.0f * Time.deltaTime);

        if(Vector3.Distance(nextMovementPoint, transform.position) <= targetReachedRadius)
        {
            CalculateNextMovementPoint();
        }
    }
    void CalculateNextMovementPoint()
    {
       /* float posX = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
        float posY = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
        float posZ = Random.Range(initialPosition.z - bound.z, initialPosition.z + bound.z); */

        nextMovementPoint = initialPosition + player.transform.position;
    }
}
