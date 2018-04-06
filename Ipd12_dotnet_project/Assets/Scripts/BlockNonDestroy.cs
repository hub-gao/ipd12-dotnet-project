using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockNonDestroy : MonoBehaviour
{
    Vector3 XPos;
    Vector3 YPos;
    Vector3 ZPos;

    private void Start()
    {
        XPos = transform.position;
        YPos = transform.position;
        ZPos = transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        // disable trigger

    }
}
