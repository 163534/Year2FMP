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
   
    private void OnTriggerEnter(Collider col)
    {
        Destroy(gameObject);
    }
}
