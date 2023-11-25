using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{

    public GameObject respawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col) {

        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {   

            SendToRespawn(col);

        }                                         

    }

    void SendToRespawn(Collider col) {

        col.transform.position = respawnPoint.transform.position;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

}
