using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIcochet : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    Vector3 lastVelocity;

    public float speedThresh = 3f;
    public float dampenerDef = 0.1f;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
        lastVelocity = rb.velocity;

    }

    void OnCollisionEnter(Collision collider) {

        var dampener = 1.0f;

        if (lastVelocity.magnitude > speedThresh) {

            if (!collider.gameObject.GetComponent<Dampen>()) {

                dampener = dampenerDef;

            } else {

                dampener = collider.gameObject.GetComponent<Dampen>().dampener;

            }

            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collider.contacts[0].normal);
            Debug.Log(speed);
            
            rb.velocity = direction * Mathf.Max(speed, 0f) * dampener;
        
        }

    }

}

////    CREDITS ////
//  - class inspired by ozkituts on YouTube
//      - https://youtu.be/RoZG5RARGF0?si=GmRS8EruBX8VC_vf&t=125