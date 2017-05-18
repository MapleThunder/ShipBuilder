using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    ObstacleCourse _Course;

    void Start()
    {
        _Course = GameObject.FindObjectOfType<ObstacleCourse>();
    }
    // Update is called once per frame
    void Update ()
    {
        Text txt = this.GetComponent<Text>();
        if (_Course.TimeRemaining > 0)
        {
            txt.text = "Time Remaining: " + _Course.TimeRemaining.ToString("0.0");
        }
        else
        {
            txt.text = "Time Remaining: 0.0";
            txt.color = Color.red;
        }
    }
}
