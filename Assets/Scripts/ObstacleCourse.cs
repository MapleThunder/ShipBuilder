using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleCourse : MonoBehaviour {

    /**
     *  Private Properties:
     *      MinDistance             The minimum distance a checkpoint must be from the previous.
     *      MaxDistance             The maximum distance a checkpoint can be from the previous.
     *      MaxAngle                The maximum angle the checkpoints can be different from the previous.
     *      _Checkpoints            A list of all the checkpoints.
     *      activeCheckpointIndex   The index of the currently active checkpoint.
     *      percentDiscount         The total percent discount earned.
     *      _CM                     A copy of the camera manager to check the game mode.
     */
    float MinDistance = 50;
    float MaxDistance = 75;
    float MaxAngle = 10;
    List<Checkpoint> _Checkpoints;
    int activeCheckpointIndex;
    int percentDiscount = 0;
    CameraManager _CM;


    /**
     *  Public Properties:
     *      CheckPointPrefab        The Prefab for the checkpoint.
     *      CourseStart             The location of the start of the course.
     *      NumberOfCheckPoints     The number of checkpoints in the course.
     *      CheckpointActiveMat     The material for an active checkpoint.
     *      CheckpointInactiveMat   The material for an inactive checkpoint.
     *      CheckpointPassedMat     The material for a checkpoint that was successfully passed.
     *      VictoryPanel            The panel that will show when you finish the course.
     *      TimeRemaining           The amount of time remaining to gain the discount.
     */
    public GameObject CheckPointPrefab;
    public GameObject CourseStart;
    public int NumberOfCheckPoints = 10;
    public Material CheckpointActiveMat;
    public Material CheckpointInactiveMat;
    public Material CheckpointPassedMat;
    public GameObject VictoryPanel;
    public Camera theCamera;
    public float TimeRemaining = 60.0f;


    /// <summary>
    /// Activates the checkpoint.
    /// </summary>
    /// <param name="cp"></param>
    void ActivateCheckpoint( Checkpoint cp)
    {
        MeshRenderer[] mrs = cp.transform.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mr in mrs)
        {
            mr.material = CheckpointActiveMat;
        }
    }

    /// <summary>
    /// Deactivates the checkpoint if it is the active one, and activates the next.
    /// </summary>
    public void CheckpointWasTriggered( Checkpoint cp )
    {
        // Grab the index of the triggered checkpoint and check if it's active.
        int thisIndex = _Checkpoints.IndexOf(cp);
        if (thisIndex != activeCheckpointIndex) return;

        // Inactivate the checkpoint and increment the activeCheckpointIndex
        InactivateCheckpoint(cp);
        activeCheckpointIndex++;
        // Increment bonus if within the time limit.
        if(TimeRemaining > 0)
        {
            percentDiscount++;
        }

        // Was that the last checkpoint ?
        if(activeCheckpointIndex >= NumberOfCheckPoints)
        {
            // If so, you're done !
            Debug.Log("ObstacleCourse:: CheckpointWasTriggered ->>> No more checkpoints !");
            Victory();
            return;
        }
        // Activate next checkpoint.
        ActivateCheckpoint(_Checkpoints[activeCheckpointIndex]);

    }

    /// <summary>
    /// Deletes all checkpoints.
    /// </summary>
    void Cleanup()
    {
        if(_Checkpoints != null)
        {
            foreach(Checkpoint cp in _Checkpoints)
            {
                Destroy(cp.gameObject);
            }

            _Checkpoints = null;
        }
    }

    /// <summary>
    /// Inactivates the checkpoint.
    /// </summary>
    /// <param name="cp"></param>
    void InactivateCheckpoint( Checkpoint cp)
    {
        MeshRenderer[] mrs = cp.transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in mrs)
        {
            mr.material = CheckpointPassedMat;
        }
    }

    /// <summary>
    /// Spawns an obstacle course to fly through.
    /// </summary>
    void SpawnCourse()
    {
        _Checkpoints = new List<Checkpoint>();
        activeCheckpointIndex = 0;

        Vector3 cpPos = CourseStart.transform.position;
        Quaternion cpRot = Quaternion.identity;

        for (int i = 0; i < NumberOfCheckPoints; i++)
        {
            // Instantiate the checkpoint and add it to the list.
            GameObject cpGO = (GameObject)Instantiate(CheckPointPrefab);
            cpGO.transform.SetParent(this.transform);
            Checkpoint cp = cpGO.GetComponent<Checkpoint>();
            cp.ObstacleCourse = this;
            _Checkpoints.Add(cp);

            // Position the checkpoint a random distance ahead 
            // and slightly offset by a random angle
            Vector3 offset = new Vector3(0, 0, Random.Range(MinDistance, MaxDistance));

            cpRot *= Quaternion.Euler(
                Random.Range(-MaxAngle, MaxAngle), 
                Random.Range(-MaxAngle, MaxAngle), 
                0);

            cpPos += cpRot * offset;
            cpGO.transform.position = cpPos;
            cpGO.transform.rotation = cpRot;
        }

        ActivateCheckpoint(_Checkpoints[activeCheckpointIndex]);
    }

	// Use this for initialization
	void Start ()
    {
        _CM = Camera.main.GetComponent<CameraManager>();
        SpawnCourse();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(_CM.Mode == Mode.Flight)
            TimeRemaining -= Time.deltaTime;
	}

    /// <summary>
    /// Fires when you complete the course.
    /// </summary>
    void Victory()
    {
        CameraManager cm = theCamera.GetComponent<CameraManager>();
        cm.SetMode(Mode.Menu);
        VictoryPanel.SetActive(true);

        Text txt = GameObject.Find("DiscountText").GetComponent<Text>();
        if(percentDiscount == 8)
            txt.text = "You won an " + percentDiscount + " discount !";
        else
            txt.text = "You won a " + percentDiscount + " discount !";
    }
}
