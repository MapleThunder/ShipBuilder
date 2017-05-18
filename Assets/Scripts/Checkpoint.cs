using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [System.NonSerialized]
    public ObstacleCourse ObstacleCourse;

    /// <summary>
    /// Calls the CheckpointWasTriggered function in the ObstacleCourse.
    /// </summary>
    void OnTriggerEnter()
    {
        ObstacleCourse.CheckpointWasTriggered(this);
    }
}
