using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStandingRotatingObject : MonoBehaviour {

    public Transform target;
    public float lookSoomth;
    public Vector3 offset;
    public float xTilt;
    Vector3 destination;
    CharacterMotorLevel1 characterMotor;
    bool isRotating;
    private void Start()
    {
        SetTarget(target);
        lookSoomth = 0.1f;
        offset = new Vector3(-15, 8, -10);//calculate offset
        xTilt = 10f;
        destination = Vector3.zero;
        isRotating = false;
    }
    private void OnTriggerEnter(Collider other)
    {

        StartCoroutine("RotateAroundTarget", 360f / 480f);

    }
    public void SetTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            if (target.GetComponent<CharacterMotorLevel1>())
            {
                characterMotor = target.GetComponent<CharacterMotorLevel1>();
            }
            else
                Debug.LogError("The rotating object's target needs a character Motor component.");
        }
        else
            Debug.LogError("Your rotating object needs a target.");
    }
   
    IEnumerator RotateAroundTarget(float angle)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 offsetFromTargetMoveAngle = Quaternion.Euler(0, angle, 0) * offset;
        float distance = Vector3.Distance(offset, offsetFromTargetMoveAngle);
        isRotating = true;
        while (distance > 0.2f)
        {
            offset = Vector3.SmoothDamp(offset,
                offsetFromTargetMoveAngle, ref velocity, lookSoomth);
            distance = Vector3.Distance(offset, offsetFromTargetMoveAngle);
            yield return null;
        }
        isRotating = false;
    }
    void MoveToTarget()
    {
        destination = characterMotor.TargetRotation * offset;
        destination += target.position;
        transform.position = destination;
    }
    void MoveWithTarget()
    {
        transform.position = Vector3.Lerp(transform.position,
                                  target.position + offset,
                                  lookSoomth);
    }
    
}
