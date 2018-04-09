using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotorLevel1 : MonoBehaviour
{

    public Transform target;
    public float lookSoomth;
    public Vector3 offsetFromTarget;
    public float xTilt;
    Vector3 destination;
    CharacterMotorLevel1 characterMotor;
    //float rotateVelocity;
    bool isRotating;
    private void Start()
    {
        SetCameraTarget(target);
        lookSoomth = 0.1f;
        offsetFromTarget = new Vector3(-15, 8, -10);
        xTilt = 10f;
        destination = Vector3.zero;
        //rotateVelocity = 0f;
        isRotating = false;
    }
    public void SetCameraTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            if (target.GetComponent<CharacterMotorLevel1>())
            {
                characterMotor = target.GetComponent<CharacterMotorLevel1>();
            }
            else
                Debug.LogError("The camera's target needs a character Motor component.");
        }
        else
            Debug.LogError("Your camera needs a target.");
    }
    private void LateUpdate()
    {
        //moving 
        if (characterMotor.grounded
            && !characterMotor.isJumping)
        {
            MoveToTarget();
        }

        //rotating
        LookAtTarget();
        //key G rotating around the left side
        if (Input.GetKeyDown(KeyCode.G) && !isRotating)
        {
            StartCoroutine("RotateAroundTarget", 30);
        }
        //key H rotating around the right side
        if (Input.GetKeyDown(KeyCode.H) && !isRotating)
        {
            StartCoroutine("RotateAroundTarget", -30);
        }
    }
    IEnumerator RotateAroundTarget(float angle)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 offsetFromTargetMoveAngle = Quaternion.Euler(0, angle, 0) * offsetFromTarget;
        //make while condition,while loop once the distance is close enough,then exit loop.
        float distance = Vector3.Distance(offsetFromTarget, offsetFromTargetMoveAngle);
        isRotating = true;
        while (distance > 0.2f)
        {
            offsetFromTarget = Vector3.SmoothDamp(offsetFromTarget,
                offsetFromTargetMoveAngle, ref velocity, lookSoomth);
            distance = Vector3.Distance(offsetFromTarget, offsetFromTargetMoveAngle);
            yield return null;
        }
        isRotating = false;
    }
    void MoveToTarget()
    {
        //Formula is S = R * θ
        //S represents the arc length, 
        //r represents the radius of the circle 
        //and θ represents the angle in radians made by the arc at the centre of the circle
        destination = characterMotor.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }
    void MoveWithTarget()
    {
        transform.position = Vector3.Lerp(transform.position,
                                  target.position + offsetFromTarget,
                                  lookSoomth);
    }
    void LookAtTarget()
    {
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSoomth);
        //problem here smooth but no focus
        //float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVelocity, lookSoomth);
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, transform.eulerAngles.z);
        //problem here no smooth using lookat
        //transform.LookAt(target);
    }
}

