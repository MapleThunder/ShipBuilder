using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : KeybindableComponent {

    /**
     *  Private Properties:
     *      _ShipRigidbody      The rigidbody of the root ship.
     */
    Rigidbody _ShipRigidbody;

    /**
     *  Public Properties:
     *      ThrusterStrength    The power of the thruster.
     */
    public float ThrusterStrength = 10;

    

    /// <summary>
    /// Fixed Update runs immediately before each tick of the physics engine.
    /// This is where all changes to physics should happen.
    /// (Exception: Totally instantaneous physical effects CAN be done in normal
    /// Update(), but it's probably less confusing if you still do them in Fixed.)
    /// </summary>
    void FixedUpdate()
    {
        if (_ShipRigidbody.isKinematic)
        {
            SetPariclesEnabled(false);
            return;
        }

        if(Input.GetKey(keyCode))
        {
            // Applying thrust
            Vector3 theForce = -this.transform.forward * ThrusterStrength;
            SetPariclesEnabled(true);
            _ShipRigidbody.AddForceAtPosition(theForce, this.transform.position);
        }
        else
        {
            // Not thrusting
            SetPariclesEnabled(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enabled"></param>
    void SetPariclesEnabled(bool enabled)
    {
        ParticleSystem.EmissionModule em = GetComponentInChildren<ParticleSystem>().emission;
        em.enabled = enabled;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start ()
    {
		_ShipRigidbody = this.transform.root.GetComponent<Rigidbody>();
    }

}
