using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flick : MonoBehaviour
{

    // scene setting
    public float timeSlow = 0.5f;
    private bool timeSlowActivated = false;

    public float camZoomIn = 0.8f;
    public float camZoomOut = 1.2f;
    private Camera cam;
    [SerializeField] private float camZoomDef;

    // movement
    private Rigidbody rb;

    public float groundFlickForce = 1300f;

    public float airFlickForce = 10f;
    public int airFlicks;
    public int airFlicksMax = 1;
    public float airTorque = -50;

    private Renderer renderer;
    
    // ground detect
    [SerializeField] private RaycastHit hit;
    public float hitDistance = 0.5f;
    [SerializeField] private bool groundCheck = false;
    private bool groundCheckDebug = false;

    // Start is called before the first frame update
    void Start()
    {
        
        name = gameObject.name;

        Debug.Log("hello from " + name);
        Debug.Log(" - time slow: " + timeSlow);
        Debug.Log(" - zoom in: " + camZoomIn);
        Debug.Log(" - zoom out: " + camZoomOut);
        Debug.Log(" - ground flick force: " + groundFlickForce);
        Debug.Log(" - air flick force: " + airFlickForce);
        Debug.Log(" - max air flicks: " + airFlicksMax);
        Debug.Log(" - air torque: " + airTorque);
        Debug.Log(" - hit distance: " + hitDistance);

        cam = Camera.main;
        camZoomDef = cam.fieldOfView;

        rb = gameObject.GetComponent<Rigidbody>();

        airFlicks = 0;
        renderer = gameObject.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (airFlicks > 0) {

            renderer.material.color = Color.blue;

        } else {

            renderer.material.color = Color.gray;

        }

        // ground detect
        groundCheck = GroundCheck(hitDistance, hit);

        if (groundCheck) { AirFlicksRestore(airFlicksMax); }


        // simple ground check debugging - message when ground touched, message when off ground
        if (groundCheck && !groundCheckDebug) {

            groundCheckDebug = true;
            Debug.Log("ground touched");
            Debug.Log("air flicks: " + airFlicks);

        } else if (!groundCheck && groundCheckDebug) {

            groundCheckDebug = false;
            Debug.Log("off the ground");

        }


        // flick while on the ground
        if (groundCheck) {

            // jump logic
            if (Input.GetButtonDown("Jump")) {                              // on jump press...

                RevertScene(1.0f, camZoomDef, true);
                SetScene(timeSlow, camZoomIn, true, 0f);                  // ..set scene up for aiming
                timeSlowActivated = true;

            } else if (Input.GetButtonUp("Jump")) {                         // otherwise, if jump released...

                RevertScene(1.0f, camZoomDef, true);                             // ..return scene to normal...
                AddForceAtAngle(groundFlickForce, transform.eulerAngles.z);           // ..and launch the cap
                timeSlowActivated = false;

            }

        }

        // flick while in air
        else {

            if (airFlicks > 0) {

                if (Input.GetButtonDown("Jump")) {                              // on jump press...

                    SetScene(timeSlow, camZoomOut, false, airTorque);                  // ..set scene up for aiming

                } else if (!timeSlowActivated && Input.GetButton("Jump")) {
                    
                    SetScene(timeSlow, camZoomOut, false, airTorque);                  // ..set scene up for aiming
                    timeSlowActivated = true;

                } else if (Input.GetButtonUp("Jump")) {                         // otherwise, if jump released...

                    RevertScene(1.0f, camZoomDef, true);                             // ..return scene to normal...
                    AddForceAtAngle(airFlickForce, transform.eulerAngles.z);           // ..and launch the cap
                    airFlicks -= 1;

                }

            }

        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.2f, Color.green, 1);   // more debugging

    }


    // function for aiming
    void SetScene(float timeSlow, float camZoom, bool gravity, float torque) {     // degree to change time flow, variable to zoom camera in by

        Debug.Log("angular velocity: " + rb.angularVelocity);
        
        if (rb.angularVelocity.z > -0.01 && rb.angularVelocity.z < 0.01) {

            if (transform.eulerAngles.z >= 0) {

                Debug.Log("torqueing counter");
                rb.angularVelocity = Vector3.zero;
                rb.AddTorque(Vector3.forward * torque);

            } else {

                Debug.Log("torqueing clockwise");
                rb.angularVelocity = Vector3.zero;
                rb.AddTorque(Vector3.forward * -torque);

            }
        
        } else if (rb.angularVelocity.z > 0) {

            Debug.Log("torqueing counter");
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(Vector3.forward * -torque);

        } else if (rb.angularVelocity.z <= 0) {

            Debug.Log("torqueing clockwise");
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(Vector3.forward * torque);

        }

        rb.useGravity = gravity;
        Time.timeScale = timeSlow;              // slow down time
        var camSavedZoom = cam.fieldOfView;     // save original camera scale to revert
        cam.fieldOfView *= camZoom;             // frame the action/suspense

    }


    // function to return scene to normal (to continue action)
    void RevertScene(float timeScale, float fieldOfView, bool gravity) {      // degree to return time flow to, variable to set camera action to

        rb.useGravity = gravity;
        Time.timeScale = timeScale;         // restore time flow
        cam.fieldOfView = fieldOfView;      // revert camera frame

    }


    // function to check if object is relatively "on the ground" - uses raycast
    bool GroundCheck(float hitDistance, RaycastHit hit) {

        // check up
        if (Physics.Raycast(transform.position, transform.up, out hit)) {                            // if raycast hits anythin...

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")                                            // and if raycast is on ground layer
                && hit.distance <= hitDistance                                                                              // and within certain distance...
                && hit.point.y < transform.position.y) {                                              // and below you...                                                                         

                return true;                                                                                            // .. then MUST be on ground

            }

        }

        // check down
        if (Physics.Raycast(transform.position, -transform.up, out hit)) {                          // if raycast hits anythin...

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")                                            // and if raycast is on ground layer
                && hit.distance <= hitDistance                                                                              // and within certain distance...
                && hit.point.y < transform.position.y) {                                              // and below you...                                                                         

                return true;                                                                                            // .. then MUST be on ground

            }

        }

        // check left
        if (Physics.Raycast(transform.position, -transform.right, out hit)) {                          // if raycast hits anythin...

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")                                            // and if raycast is on ground layer
                && hit.distance <= hitDistance                                                                              // and within certain distance...
                && hit.point.y < transform.position.y) {                                              // and below you...                                                                         

                return true;                                                                                            // .. then MUST be on ground

            }

        }

        // check right
        if (Physics.Raycast(transform.position, transform.right, out hit)) {                          // if raycast hits anythin...

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")                                            // and if raycast is on ground layer
                && hit.distance <= hitDistance                                                                              // and within certain distance...
                && hit.point.y < transform.position.y) {                                              // and below you...                                                                         

                return true;                                                                                            // .. then MUST be on ground

            }

        }

        return false;

    }


    public void AirFlicksRestore(int airFlicksRestore) {

        if (airFlicks < airFlicksRestore) {
                    
            airFlicks = airFlicksRestore;

        } else {

            airFlicks = airFlicks;

        }

    }


    // 1. adapted from Unity Discussions - function to launch object given angle (z-axis for our scope)
    void AddForceAtAngle(float force, float angle) {     // power at which to launch rigidbody, angle at which to launch

        float yComponent = Mathf.Cos(angle * Mathf.PI / 180) * force;   // vertical direction
        float xComponent = -Mathf.Sin(angle * Mathf.PI / 180) * force;  // horizontal direction; negative to launch in direction of "top"

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (rb.angularVelocity.z > 0) {

            Debug.Log("torqueing counter");
            rb.AddTorque(Vector3.forward * -airTorque * 2);

        } else if (rb.angularVelocity.z <= 0) {

            Debug.Log("torqueing clockwise");
            rb.AddTorque(Vector3.forward * airTorque * 2);

        }

        rb.AddForce(xComponent, yComponent, 0);     // launch rigidbody

    }

}

////    CREDITS ////
//  1. addForceAtAngle from GlassesGuy on Unity Discussions
//      -- https://discussions.unity.com/t/can-you-add-a-force-to-a-rigidbody-at-an-angle/225206 
