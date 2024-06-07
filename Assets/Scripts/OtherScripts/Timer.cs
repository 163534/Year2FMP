using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool timerActive;
    private float currentTime;
    [SerializeField]
    private TMP_Text textVar;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        timerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        
        TimeSpan time = TimeSpan.FromSeconds(currentTime);

        textVar.text = "Time " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
    }
}
