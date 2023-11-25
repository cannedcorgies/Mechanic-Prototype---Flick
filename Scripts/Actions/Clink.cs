using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clink : MonoBehaviour
{

    public int flickStock = 1;

    // Start is called before the first frame update
    void Start()
    {
        
        name = gameObject.name;

        Debug.Log("hello from " + name);
        Debug.Log(" - clinks stock: " + flickStock);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collider) {

        if (collider.gameObject.GetComponent<Flick>()) {   

            collider.gameObject.GetComponent<Flick>().AirFlicksRestore(flickStock);

        } 

    }

}
