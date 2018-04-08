using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<Renderer>().enabled = false;
        }
    }
}
