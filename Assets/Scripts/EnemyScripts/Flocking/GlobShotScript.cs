using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobShotScript : MonoBehaviour
{
    GameObject player;
    public Transform startPosition;
    Vector3 playerPosition;
    public float slerpSpeed;
    public float startTime;
    public float journeyTime;
    public Transform sunrise;
    public Transform sunset;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        startTime = Time.time;
        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform.position;
        startPosition = gameObject.transform;
        //StartCoroutine(SlerpTowardsPlayer());
    }
    private void Update()
    {
        EvaluateSlerpPoints(startPosition.position, playerPosition, -1);
    }

    // Update is called once per frames
    IEnumerator SlerpTowardsPlayer()
    {
        float time = 0;
        while (time< 1)
        {
            //transform.position = Vector3.Slerp(startPosition.position, playerPosition, time);
            EvaluateSlerpPoints(startPosition.position, playerPosition, 0);
            print("message");
            time += Time.deltaTime * slerpSpeed;
            yield return null;
        }
    }
    void EvaluateSlerpPoints(Vector3 start, Vector3 end, float centerOffset)
    { 
        var centerPivot = (start + end) * 0.5f;

        centerPivot -= new Vector3(0, -centerOffset, 0);

        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;

        var fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(startRelativeCenter, endRelativeCenter, fracComplete);
        transform.position += centerPivot;

    }
    void Slerp()
    {
        // The center of the arc
        Vector3 center = (sunrise.position + sunset.position) * 0.5F;

        // move the center a bit downwards to make the arc vertical
        center -= new Vector3(0, 1, 0);

        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = sunrise.position - center;
        Vector3 setRelCenter = sunset.position - center;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            print("Hit player");
            Destroy(gameObject);
        }
    }
}
