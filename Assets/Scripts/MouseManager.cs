using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle the mouse interactions.
/// </summary>
public class MouseManager : MonoBehaviour {

    /**
     *  Private Properties:
     *      _ShipRoot       The ship root GameObject instantiated.
     */

    private GameObject _ShipRoot;

    /**
     *  Public Properties
     *      PrefabToSpawn           The Prefab of the part to be spawned on click.
     *      SnapPointLayerMask      The layer mask to determine what snap points are.
     *      ComponentKeybindDialog  The window that will display to set key binds.
     *      ShipRoots               An array of two root ship GameObjects.
     *      TheCamera               The main camera.
     */
    public GameObject PrefabToSpawn;
    public LayerMask SnapPointLayerMask;
    public ComponentKeybindDialog ComponentKeybindDialog;
    public GameObject[] ShipRoots;
    public Camera TheCamera;

    /// <summary>
    /// Adds a part to the selected collider.
    /// </summary>
    /// <param name="theCollider"></param>
    void AddPart(Collider theCollider)
    {
        Vector3 spawnSpot = theCollider.transform.position;
        Quaternion spawnRotation = theCollider.transform.rotation;

        // Now spawn a new object
        GameObject GO = (GameObject)Instantiate(PrefabToSpawn, spawnSpot, spawnRotation);
        GO.transform.SetParent(theCollider.transform);

        // To calculate mass, get a list of all the colliders in the new part.
        Collider[] colliders = GO.transform.GetComponentsInChildren<Collider>();
        float mass = 0;
        foreach(Collider col in colliders)
        {
            int bitMaskForCollider = 1 << col.gameObject.layer;
            if ((bitMaskForCollider & SnapPointLayerMask) == 0)
            {
                // This collider is NOT a snap point, so calculate it's mass contribution.
                float volume = col.bounds.size.x * col.bounds.size.y * col.bounds.size.z;
                mass += volume;
            }

        }

        theCollider.transform.GetComponentInParent<Rigidbody>().mass += mass;

        // Disable the renderer on the snap point when something is created there.
        if (theCollider.GetComponent<Renderer>() != null)
            theCollider.GetComponent<Renderer>().enabled = false;
        if (theCollider.GetComponent<Collider>())
            theCollider.GetComponent<Collider>().enabled = false;
    }

    /// <summary>
    /// Adds a selected part to the ship.
    /// </summary>
    void CheckLeftClick()
    {

        Collider theCollider = DoRaycast();
        if (theCollider == null) return;

        // Did we click on a snap point?
        // Our snap points are always going to be on the SnapPoint physics layer

        // The normal integer number value of 8 is represented thusly in binary:
        //
        //      0000000000000000001000
        //
        // When you're talking about things like "LayerMasks" "BitMasks" "Flags"
        //
        //      00000000000000100010000 (in this case "Water" and "SnapPoint" are on)
        //      Would represent something like the 4th & 8th item in the list is enabled
        //      (The layermask is zero-indexed, so layer 4 & 8 are the 5th and 9th bit.)
        //
        // Bitwise operators and comparison
        //
        //  &: This kind of compares two integers and returns a new integer where
        //     both had their bits enabled
        //      Example:
        //              10110 
        //            & 10011
        //              ----
        //              10010
        //
        //  |: This is a bitwise or
        //
        //              10110 
        //            | 10011
        //              -----
        //              10111
        //
        //
        //  <<: The left-shift operator.
        //
        //     1 << 8
        //
        //    Goes from this: 0000000000000000001
        //    To this:        0000000000100000000
        //

        int bitMaskForLayer = 1 << theCollider.gameObject.layer;

        // Did I hit a snap point ?
        // Snap Points are always on their own layer.
        if ((bitMaskForLayer & SnapPointLayerMask) != 0)
        {
            AddPart(theCollider);
        }
        
    }

    /// <summary>
    /// Checks for what is under the pointer at time of click, and
    /// handles all functionality to the click.
    /// </summary>
    void CheckRightClick()
    {
        Collider theCollider = DoRaycast();
        if (theCollider == null) return;

        // Right clicked on something. Is it Keybindable ?
        // Need to check the PARENT of the object with the collider, because of how the prefabs are set up.
        GameObject shipPart = FindShipPart(theCollider);
        if(shipPart == null)
        {
            Debug.Log("MouseManager::CheckRightClick ->>> Part has no parent.");
            // Clicked on something without a parent, so it probably isn't a part of the ship
            return;
        }

        KeybindableComponent kc = shipPart.GetComponent<KeybindableComponent>();
        if (kc == null)
        {
            Debug.Log("MouseManager::CheckRightClick ->>> Part not keybindable.");
            // This object is not keybindable.
            return;
        }

        // If it gets here, I have right-clicked on something keybindable.
        ComponentKeybindDialog.OpenDialog(kc);
        
    }

    /// <summary>
    /// Returns the collider of whatever is hit by a ray.
    /// </summary>
    /// <returns></returns>
    Collider DoRaycast()
    {
        Ray ray = TheCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider;
        }

        return null;
    }

    /// <summary>
    /// Finds the ship part parent of the collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    GameObject FindShipPart(Collider collider)
    {
        Transform curr = collider.transform;

        while(curr != null)
        {
            if(curr.gameObject.tag == "ShipPart")
            {
                return curr.gameObject;
            }

            curr = curr.parent;
        }

        return null;
    }

    /// <summary>
    /// Removes a part, and all child parts.
    /// </summary>
    /// <param name="go"></param>
    void RemovePart(GameObject go)
    {
        go.transform.parent.GetComponent<Renderer>().enabled = true;
        go.transform.parent.GetComponent<Collider>().enabled = true;

        Destroy(go);
    }

    /// <summary>
    /// Unlocks the camera, shows snap points, and enables right-click functionality.
    /// </summary>
    public void SetMode_Edit()
    {
        // Show all snap points
        SetSnapPointEnabled(_ShipRoot.transform, true);

        // Unlock the camera control.
        TheCamera.transform.parent.SetParent(null);
        CameraManager cm = TheCamera.GetComponent<CameraManager>();
        cm.SetMode(Mode.Edit);
    }

    /// <summary>
    /// Locks the camera to prepare for flight and hides snap points.
    /// </summary>
    public void SetMode_Flight()
    {
        // Hide all snap points
        SetSnapPointEnabled(_ShipRoot.transform, false);

        // Tell the camera to lock to ship root.
        TheCamera.transform.parent.SetParent(_ShipRoot.transform);
        TheCamera.transform.parent.localPosition = Vector3.zero;
        CameraManager cm = TheCamera.GetComponent<CameraManager>();
        cm.SetMode(Mode.Flight);

    }

    /// <summary>
    /// Enables a snap point.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="setToActive"></param>
    void SetSnapPointEnabled( Transform t, bool setToActive)
    {
        int maskForThisHitObject = 1 << t.gameObject.layer;

        if((maskForThisHitObject & SnapPointLayerMask) != 0)
        {
            // This is a snap point.
            if (setToActive)
            {
                // Always activate -- just in case.
                t.gameObject.SetActive(true);
            }
            else
            {
                // Only inactivate the SnapPoint if it has no children (i.e. it's on the outside of the ship.)
                if (t.childCount == 0)
                    t.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            SetSnapPointEnabled(t.GetChild(i), setToActive);
        }
    }

    void Start()
    {
        int shipIndex = PlayerPrefs.GetInt("shipIndex");
        _ShipRoot = ShipRoots[shipIndex];

        Instantiate(_ShipRoot, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        // Check to see if the mouse was pressed down this frame.
        if (Input.GetMouseButtonDown(0))
        {
            CheckLeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CheckRightClick();
        }
	} // End Update
}
