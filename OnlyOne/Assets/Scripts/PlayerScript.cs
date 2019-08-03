using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Space(10)]
    [Header("Environment")]
    public float gravity;

    [Space(10)]
    [Header("Movement")]
    public float topSpeed;
    public int tractionFrames;
    public AnimationCurve traction;
    public int slowdownFrames;
    public AnimationCurve slowdown;


    new Rigidbody rigidbody;
    Controls controls;
    Vector3 move;
    Vector3 nextVelocity;
    int frameCount;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new Controls();
        rigidbody = GetComponent<Rigidbody>();
        move = Vector3.zero;

        controls.Player.Walk.started += ctx => Move(ctx.ReadValue<float>());
        controls.Player.Walk.canceled += ctx => Move(0);
    }

    void onEnable()
    {
        controls.Player.Enable();
    }

    void onDisable()
    {
        controls.Player.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        // Gravity
        rigidbody.velocity += Vector3.down * gravity * Time.deltaTime;
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
        Debug.Log(move.x);
    }    
}
