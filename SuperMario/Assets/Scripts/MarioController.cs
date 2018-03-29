using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    public float velocity;
    // Use this for initialization
    void Start()
    {
        velocity = 100f;
        print("I am Mario.");
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * velocity,
            0f,
            Input.GetAxis("Vertical") * Time.deltaTime * velocity);
    }
}
