using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Environment")]
    public float gravity;

    [Header("Movement")]
    public float topSpeed;
    public float topJump;
    public int tractionFrames;
    public AnimationCurve traction;
    public int slowdownFrames;
    public AnimationCurve slowdown;

    
    [Header("Jump")]
    public float jumpForce;
    public AnimationCurve jumpCurve;


    new Rigidbody rigidbody;
    Controls controls;
    Vector3 move;
    Vector3 nextVelocity;
    int frameCount;
    bool airborne;
    bool jumpCommand;
    bool goingUp;
    float jumpingThreshold;
    float jumpFrame;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new Controls();
        rigidbody = GetComponent<Rigidbody>();
        
        move = Vector3.zero;
        airborne = false;
        jumpingThreshold = gravity * Time.fixedDeltaTime + 0.01f;

        controls.Player.Walk.started += ctx => Move(ctx.ReadValue<float>());
        controls.Player.Walk.canceled += ctx => Move(0);

        controls.Player.Jump.started += ctx => Jump(true);
        controls.Player.Jump.canceled += ctx => Jump(false);
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        airborne = jumpCommand || Mathf.Abs(rigidbody.velocity.y) > jumpingThreshold;
        // Gravity
        if (goingUp)
        {
            nextVelocity = rigidbody.velocity;
            nextVelocity.y += jumpForce * jumpCurve.Evaluate(jumpFrame);
            rigidbody.velocity = nextVelocity;
            
            jumpFrame += Time.deltaTime;
            goingUp = jumpFrame < jumpCurve[1].time;
        }
        else
        {
            rigidbody.velocity += Vector3.down * gravity * Time.deltaTime;
        }
        Debug.Log(goingUp);
    }

    void FixedUpdate()
    {
        // Move
        if (move.x != 0)
        {
            frameCount = Mathf.Min(frameCount + 1, tractionFrames);
            nextVelocity = rigidbody.velocity;
            nextVelocity.x = topSpeed * traction.Evaluate(frameCount / tractionFrames) * move.x;
            rigidbody.velocity = nextVelocity;
        }
        else if (rigidbody.velocity.x != 0f)
        {
            frameCount = Mathf.Min(frameCount + 1, tractionFrames);
            nextVelocity = rigidbody.velocity;
            nextVelocity.x = topSpeed * traction.Evaluate(frameCount / slowdownFrames);
            rigidbody.velocity = nextVelocity;
        }
    }

    void Move(float multiplier)
    {
        if ((move.x == 0 && multiplier != 0) ||
            (move.x != 0 && multiplier == 0))
        {
            // Moving State changed
            frameCount = 0;
        }
        move.x = multiplier;
    }    

    void Jump(bool status)
    {
        jumpCommand = status;
        goingUp = jumpCommand && !airborne;
        if (goingUp) jumpFrame = 0;
    }  
}
