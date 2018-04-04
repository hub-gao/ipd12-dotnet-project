using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//main requirement
// 1. 8-directional movement
public class CharacterController : MonoBehaviour
{
    public float inputDelay;
    public float forwardVelocity;
    public float rotateVelocity;
    Rigidbody rigidBody;
    Quaternion targetRotation;

    float forwardInput, turnInput, jumpInput;
    Animator animator;

    //for climb slope
    //the height of character
    public float height;
    public float heightPadding;
    public LayerMask ground;
    public float maxGroudAngle;
    public bool debug;
    public float groundAngel;
    public Vector3 forward;
    public RaycastHit hitInfo;
    public bool grounded;

    public float stability;
    public float speed;
    public float jumpForce;
    public bool isJumping;
    public bool isWalking;
    public Quaternion TargetRotation
    {
        get
        {
            return targetRotation;
        }
    }
    void Start()
    {
        //initialization
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("The character needs the Rigidbody component.");
        }
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("The charater needs Animator component.");
        }
        forwardInput = 0;
        turnInput = 0;
        jumpInput = 0;
        inputDelay = 0.1f;
        forwardVelocity = 5;
        rotateVelocity = 100;
        height = 0.1f;
        heightPadding = 0.05f;
        maxGroudAngle = 120;
        ground = 1 << 8; //layer the ground at position 8 and set ...0000 1000 0000
        debug = true;

        stability = 3f;
        speed = 15f;
        jumpForce = 10f;
        isJumping = false;
        isWalking = false;
    }
    void GetInput()
    {
        //forward or backward
        forwardInput = Input.GetAxis("Vertical");
        //turn left or right
        turnInput = Input.GetAxis("Horizontal");
        //jump
        jumpInput = Input.GetAxis("Jump");
    }

    private void Update()
    {
        GetInput();
        CheckGround();
        CalculateForward();
        CalculateGroundAngle();
        ApplyGravity();
        DrawDebugLines();
        Walk();
        Turn();
    }
    void FiexdUpdate()
    {
        //stability
        Vector3 predictedUp = Quaternion.AngleAxis(
           rigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
           rigidBody.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        // Uncomment the next line to stabilize on only 1 axis.
        //torqueVector = Vector3.Project(torqueVector, transform.forward);
        rigidBody.AddTorque(torqueVector * speed);
    }
    private void CheckGround()
    {
        if (Physics.Raycast(transform.position,
            -Vector3.up,
            //here, getting the hitInfo for check ground angle and calculate forward
            out hitInfo,
            height + heightPadding,
            // the ground is layer mask which is telling us we are actually on ground or not
            ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    private void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }
        //The cross product of two vectors results in a third vector which is perpendicular to the two input vectors.
        forward = Vector3.Cross(hitInfo.normal, -transform.right);
    }

    private void CalculateGroundAngle()
    {
        if (!grounded)
        {
            //the angle between forward and up
            groundAngel = 90;
            return;
        }
        groundAngel = Vector3.Angle(hitInfo.normal, transform.forward);
    }


    private void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
        else
        {
            Jump();
        }
    }
    private void DrawDebugLines()
    {
        if (!debug) return;
        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

    void Walk()
    {
        if (groundAngel < maxGroudAngle
            && Mathf.Abs(forwardInput) > inputDelay)
        {
            //moving
            rigidBody.velocity = forward * forwardInput * forwardVelocity;
            animator.SetBool("isWalk", true);
            isWalking = true;
        }
        else
        {
            //here fix jitter error
            rigidBody.velocity = Vector3.zero;
            animator.SetBool("isWalk", false);
            isWalking = false;
        }
    }
    void Jump()
    {
        if (Mathf.Abs(jumpInput) > inputDelay)
        {
            Vector3 moveVector = new Vector3(0,jumpForce,0);
            transform.Move();
        }
    }


    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            //rotating
            targetRotation *= Quaternion.AngleAxis(rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }
}
