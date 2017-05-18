using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustField : MonoBehaviour {

    /**
     *  Public Properties:
     *      NumberOfDustMotes   Total number of motes spawned.
     *      DustMotePrefab      The Prefab for the dust mote to be spawned.
     *      TheCamera           The main camera.
     *      CloudRadius         The radius of the cloud of dust motes.
     */
    public int NumberOfDustMotes = 100;
    public GameObject DustMotePrefab;
    public Transform TheCamera;
    public float CloudRadius = 25f;

	/// <summary>
    /// Creates initial dust cloud around ship.
    /// </summary>
	void Start ()
    {
        // Make sure there is a DustMotePrfab
        if(DustMotePrefab == null)
        {
            Debug.LogError("DustField::Start ->>> No DustMotePrefab found.");
            return;
        }

        // Make sure there is a reference to the camera
        if(TheCamera == null)
        {
            // Try and grab the main camera
            TheCamera = Camera.main.transform;

            if(TheCamera == null)
            {
                Debug.LogError("DustField::Start ->>> No Camera found.");
                return;
            }

        }

        MeshRenderer mr = DustMotePrefab.transform.GetComponentInChildren<MeshRenderer>();
        Material spaceDustMat = mr.sharedMaterial;
        spaceDustMat.SetFloat("_FalloffDistance", CloudRadius);

        for (int i = 0; i < NumberOfDustMotes; i++)
        {
            Vector3 dustMotePosition = TheCamera.position +
                (Random.insideUnitSphere * CloudRadius);

            Instantiate(DustMotePrefab, dustMotePosition, Random.rotation, this.transform);
        }
		
	}
	
	/// <summary>
    /// When dust motes gets too far away, move it to the other side of the camera.
    /// </summary>
	void Update ()
    {
        // This is to avoid computing a square root for every single dust mote
        // multiple times a frame. This way I can compare the squared values instead.
        float maxDistanceSquared = CloudRadius * CloudRadius;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            // Grab the child transform
            Transform theChild = this.transform.GetChild(i);
            // Is this child too far away from the camera ?
            Vector3 theDiff = theChild.position - TheCamera.position; 

            if(theDiff.sqrMagnitude > maxDistanceSquared)
            {
                // Dust mote is too far away !
                // flip it to the other side of the camera.
                theDiff = Vector3.ClampMagnitude(theDiff, CloudRadius);
                theChild.position = TheCamera.position - theDiff;
            }
        }
	}
}
