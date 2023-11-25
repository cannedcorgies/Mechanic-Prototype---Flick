using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dampen : MonoBehaviour
{

    public float dampener = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        name = gameObject.name;

        Debug.Log("hello from " + name);
        Debug.Log(" - my dampness " + dampener);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
