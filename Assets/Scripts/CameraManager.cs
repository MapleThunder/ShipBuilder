using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { Edit, Flight, Menu }

public class CameraManager : MonoBehaviour {

    /**
     *  Private Properties:
     *      _Rig                The rig that the camera rotates around.
     *      _PreviousMousePos   A placeholder for the last position of the mouse.
     *      _MinZoomDistance    The closest the camera can get to the ship.
     *      _MaxZoomDistance    The farthest the camera can zoom out from the ship.
     *      _CurrentMode        The current game mode.
     */

    private Transform _Rig;
    private Vector3 _PreviosMousePos;
    private float _MinZoomDistance = 2;
    private float _MaxZoomDistance = 25;
    Mode _CurrentMode = Mode.Edit;


    /**
     *  Public Properties:
     *      Camera              The main camera.
     *      OrbitSensitivity    How quickly the orbit moves, lower is faster.
     *      ZoomMultiplier      How quickly the zoom moves.
     *      InvertZoom          Invert the zoom axis.
     *      panSpeed            The speed at which the camera pans side to side.
     */
    public Camera theCamera;
    public float OrbitSensitivity = 10;
    public float ZoomMultiplier = 2;
    public bool InvertZoom = false;
    public float PanSpeed = 0.1f;
    public Mode Mode { get { return _CurrentMode; } }


    /// <summary>
    /// This "Zooms" the camera.
    /// </summary>
    void DollyCamera()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");
        if (InvertZoom) delta = -(delta);

        // Move the camera backwards or forwards based on the value of delta.
        Vector3 actualChange = theCamera.transform.localPosition * ZoomMultiplier * delta;
        // Add option for inverting zoom.
        

        Vector3 newPosition = theCamera.transform.localPosition + actualChange;

        newPosition = newPosition.normalized * Mathf.Clamp(newPosition.magnitude, _MinZoomDistance, _MaxZoomDistance);

        theCamera.transform.localPosition = newPosition;
    }

    /// <summary>
    /// This orbits the camera around a point.
    /// </summary>
    void OrbitCamera ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // The mouse button was pressed ON THIS FRAME.
            _PreviosMousePos = Input.mousePosition;
        }

	    if(Input.GetMouseButton(1))
        {
            // Currently holding the right mouse button

            // What is the current position of the mouse on the screen ?
            Vector3 currentMousePos = Input.mousePosition;

            Vector3 mouseMovement = currentMousePos - _PreviosMousePos;

            // I want to rotate the camera around the camera rig.

            Vector3 rotationAngles = mouseMovement / OrbitSensitivity;

            theCamera.transform.RotateAround(_Rig.position, theCamera.transform.right, -rotationAngles.y);
            theCamera.transform.RotateAround(_Rig.position, theCamera.transform.up, rotationAngles.x);

            // Make sure the camera is still focused on the _Rig.
            Quaternion lookRotation = Quaternion.LookRotation(-theCamera.transform.localPosition);
            theCamera.transform.rotation = lookRotation;

            _PreviosMousePos = currentMousePos;

        }
	}

    /// <summary>
    /// Pans the camera side to side.
    /// </summary>
    void PanCamera()
    {
        // Grab the direction of the pan.
        Vector3 panAngle = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 actualChange = panAngle * PanSpeed;

        actualChange = Quaternion.Euler(0, theCamera.transform.rotation.eulerAngles.y, 0) * actualChange;

        Vector3 newPosition = theCamera.transform.localPosition + actualChange;

        newPosition = newPosition.normalized * Mathf.Clamp(newPosition.magnitude, _MinZoomDistance, _MaxZoomDistance);

        theCamera.transform.localPosition = newPosition;
    }

    /// <summary>
    /// Sets the current mode.
    /// </summary>
    /// <param name="mode"></param>
    public void SetMode(Mode mode)
    {
        _CurrentMode = mode;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        if (theCamera == null)
            theCamera = GetComponent<Camera>();

        if (theCamera == null)
        {
            Debug.LogError("CameraManager::Start ->>> No camera found.");
            return;
        }

        _Rig = theCamera.transform.parent;

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (_CurrentMode == Mode.Edit)
        {
            OrbitCamera();
            DollyCamera();
            PanCamera();
        }
    }
}
