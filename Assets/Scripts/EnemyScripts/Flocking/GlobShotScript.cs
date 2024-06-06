using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobShotScript : MonoBehaviour
{
    GameObject player;
    public Vector3 start, end;
    public float slerpSpeed;
    public float startTime;
    public float journeyTime;

    public float offsetX, offsetY, offsetZ;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        startTime = Time.time;
        player = GameObject.FindWithTag("Player");
        
        start = transform.position;
        end = player.gameObject.transform.position + new Vector3(offsetX,offsetY,offsetZ);

    }
    private void Update()
    {
        EvaluateSlerpPoints();
    }

    // Update is called once per frames
    void EvaluateSlerpPoints()
    { 
        Vector3 centerPivot = (start + end) * 0.5f;

        centerPivot -= new Vector3(0, 0.5f, 0);

        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;

        var fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(startRelativeCenter, endRelativeCenter, fracComplete);
        transform.position += centerPivot;

    }
    void Slerp()
    {
        // The center of the arc
        Vector3 center = (start + end) * 0.5F;

        // move the center a bit downwards to make the arc vertical
        center -= new Vector3(0, 1, 0);

        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = start - center;
        Vector3 setRelCenter = end - center;

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
