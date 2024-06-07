using System.Collections;
using UnityEngine;

public class Flocking : MonoBehaviour
{

    enum BoidState
    {
        Swarm,
        Attack,
    }

    [Header("Movement variables.")]
    public float minSpeed = 20f;
    public float turnSpeed = 20f;
    public float randomFreq = 20f;
    public float randomForce = 20f;

    //alignment variables
    [Header("Alignment variables")]
    [Tooltip("This changes how strong the boids are pulled towards the centre of the controller")]
    public float toOriginForce = 50f;
    [Tooltip("This changes how far the flock spreads out from the controller")]
    public float toOriginRange = 1f;

    public float gravity = 2.0f;

    //seperation variables
    [Header("Seperation variables")]
    [Tooltip("This changes how far boids can fly to avoid each other")]
    public float avoidanceRadius = 50f;
    [Tooltip("This changes how strong the force of repulsion is")]
    public float avoidanceForce = 20f;

    //cohesion variables
    [Header("Cohesion variables")]
    [Tooltip("This changes how fast the flock will move towards the controller")]
    public float followVelocity = 4f;
    [Tooltip("This changes how far away the flock can spread out from the controller as it moves")]
    public float followRadius = 40f;

    //These variables control the movement of the boid
    private Transform origin;

    private Vector3 velocity;
    private Vector3 normalisedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private Flocking[] otherFlocks;
    private Transform transformComponent;
    private float randomFreqInterval;

    float shootCounter;

    GameObject player;
    public GameObject globShot;
    FlockingController fc;
    public Transform globShotTransform;

    BoidState state;

    // Start is called before the first frame update
    void Start()
    {
        shootCounter = Random.Range(2,6);
        fc = gameObject.GetComponentInParent< FlockingController >();
        player = GameObject.FindWithTag("Player");
        state = BoidState.Swarm;
        SwarmAssignemt();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BoidState.Swarm)
        {
            //print("Swarm is in the swarming state");
            SwarmBehaviour();
            CheckForStateChange();
        }

        if (state == BoidState.Attack)
        {
            //print("Swarn is in the attack state");
            CheckForStateChange();
            LookAtPlayer();
            ShootPlayer();

        }
    }
    void ShootPlayer()
    {
        shootCounter -= Time.deltaTime;
        //print(shootCounter);
        if(shootCounter <= 0)
        {
            GameObject globShotClone = Instantiate(globShot, globShotTransform);

            shootCounter = 4;
        }
    }

    IEnumerator UpdateRandom()
    {
        while (true)
        {
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreqInterval + Random.Range(-randomFreqInterval / 2.0f, randomFreqInterval / 2.0f));
        }
    }
    void CheckForStateChange()
    {
        if (fc.state == boidControllerState.Attack)
        {
            state = BoidState.Attack;
        }
        else if (fc.state == boidControllerState.Chase)
        {
            state = BoidState.Swarm;
        }
    }
    void SwarmAssignemt()
    {
        fc = GetComponentInParent<FlockingController>();

        randomFreqInterval = 1.0f / randomFreq;

        //Assign the parent as origin
        origin = transform.parent;

        //Flock transform
        transformComponent = transform;

        //Temporary components
        Component[] tempFlocks = null;

        //Get all the Flocking components from the parent transform in the group
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<Flocking>();
        }
        // Assign and store all the flock objects in this group
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new Flocking[tempFlocks.Length];

        for (int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (Flocking)tempFlocks[i];
        }
        //Null Parent as the flock leader will be FlockingController object
        transform.parent = null;

        //Calculate random push depends on the random freqency provided
        StartCoroutine(UpdateRandom());
    }
    void SwarmBehaviour()
    {
        //Internal variables
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        int count = 0;

        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;

        for (int i = 0; i < objects.Length; i++)
        {
            Transform boidTransform = objects[i];
            if (boidTransform != transformComponent)
            {
                Vector3 otherPosition = boidTransform.position;

                //Average position to calculate cohesion
                avgPosition += otherPosition;
                count++;

                //Directional vector from other flock to this flock
                forceV = myPosition - otherPosition;

                //Magnitude of that directional vector(Length)
                float directionMagnitude = forceV.magnitude;
                float forceMagnitude = 0.0f;

                if (directionMagnitude < followRadius)
                {
                    if (directionMagnitude < avoidanceRadius)
                    {
                        forceMagnitude = 1.0f - (directionMagnitude / avoidanceRadius);

                        if (directionMagnitude > 0)
                        {
                            avgVelocity += (forceV / directionMagnitude) * forceMagnitude * avoidanceForce;
                        }
                    }

                    forceMagnitude = directionMagnitude / followRadius;
                    Flocking tempOtherBoid = otherFlocks[i];
                    avgVelocity += followVelocity * forceMagnitude * tempOtherBoid.normalisedVelocity;
                }
            }
        }

        if (count > 0)
        {
            //Calculate the average flock velocity (alignment)
            avgVelocity /= count;

            //Calculate Center value of the flock (cohesion)
            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }
        //Directional Vector to the leader
        forceV = origin.position - myPosition;
        float leaderDirectionMagnitude = forceV.magnitude;
        float leaderForceMagnitude = leaderDirectionMagnitude / toOriginRange;

        //Calculate the velocity of the flock to the leader
        if (leaderForceMagnitude > 0)
        {
            originPush = leaderForceMagnitude * toOriginForce * (forceV / leaderDirectionMagnitude);
        }
        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }
        Vector3 wantedVel = velocity;

        //Calculate final velocity
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += gravity * Time.deltaTime * toAvg.normalized;

        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.00f);
        transformComponent.rotation = Quaternion.LookRotation(velocity);

        //Move the flock based on the calculated velocity
        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        normalisedVelocity = velocity.normalized;
    }
    void LookAtPlayer()
    {
        var rotation = Quaternion.LookRotation(player.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2.5f);
    }
}
