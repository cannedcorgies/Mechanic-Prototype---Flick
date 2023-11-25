using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;
    public Vector3 offset;
    [SerializeField] private Vector3 targetOffset;

    // Start is called before the first frame update
    void Start()
    {

        var targetName = target.name;

        Debug.Log("Hello from camera");
        Debug.Log(" - following " + targetName);

        offset = transform.position;
        targetOffset = target.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = (target.transform.position - targetOffset) + offset;

    }
}
