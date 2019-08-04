using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    [Header("Environment")]
    public float gravity;

    [Header("Movement")]
    public float topSpeed;
    public float tractionFrames;
    public AnimationCurve traction;
    public float slowdownFrames;
    public AnimationCurve slowdown;

    
    [Header("Jump")]
    public float jumpForce;
    public AnimationCurve jumpCurve;

    [Header("Shoot")]
    public Transform limbs;
    public Texture2D aimSprite;
    public GameObject ArrowPrefab;
    public int maxShots = 1;
    public AudioSource AudioPlayer;
    public AudioClip ShotFiredAudio;
    public AudioClip ReloadingAudio;
    public UnityEvent onShot;
    public UnityEvent onRetrieve;

    new Rigidbody rigidbody;
    Controls controls;
    Vector3 move;
    Vector3 nextVelocity;
    Vector2 mousePosition;
    float frameCount;
    bool airborne;
    bool jumpCommand;
    bool goingUp;
    float jumpingThreshold;
    float jumpFrame;
    int availableArrows;

    List<GameObject> ShotArrows;

    void Awake()
    {
        controls = new Controls();
        rigidbody = GetComponent<Rigidbody>();


        ShotArrows = new List<GameObject>();
        frameCount = slowdownFrames;
        move = Vector3.zero;
        airborne = false;
        jumpingThreshold = gravity * Time.fixedDeltaTime + 0.01f;
        availableArrows = maxShots;

        controls.Player.Walk.started += ctx => Move(ctx.ReadValue<float>());
        controls.Player.Walk.canceled += ctx => Move(0);

        controls.Player.Jump.started += ctx => Jump(true);
        controls.Player.Jump.canceled += ctx => Jump(false);

        controls.Player.Shoot.started += ctx => Shoot();

        controls.Player.Aim.performed += ctx => SetMousePosition(ctx.ReadValue<Vector2>());

        Cursor.SetCursor(aimSprite, Vector2.zero, CursorMode.Auto);
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
            nextVelocity = rigidbody.velocity;
            nextVelocity.y -= gravity * Time.deltaTime;
            rigidbody.velocity = nextVelocity;
        }

        // Aim
        limbs.LookAt(mousePosition);
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
            frameCount = Mathf.Min(frameCount + 1, slowdownFrames);
            nextVelocity = rigidbody.velocity;
            nextVelocity.x = topSpeed * slowdown.Evaluate(frameCount / slowdownFrames) * Mathf.Sign(rigidbody.velocity.x);
            rigidbody.velocity = nextVelocity;
        }
    }

    void SetMousePosition(Vector2 pos)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -Camera.main.transform.position.z));
    }

    void Move(float multiplier)
    {
        if ((move.x == 0 && multiplier != 0) ||
            (move.x != 0 && multiplier == 0))
        {
            // Moving State changed
            frameCount = 0f;
        }
        move.x = multiplier;
    }

    void Jump(bool status)
    {
        jumpCommand = status;
        goingUp = jumpCommand && !airborne;
        if (goingUp) jumpFrame = 0;
    }

    void Shoot()
    {
        if (availableArrows > 0)
        {
            var arrow = Instantiate(
                ArrowPrefab, 
                transform.position, 
                Quaternion.identity);
            arrow.transform.LookAt(mousePosition);
            onShot?.Invoke();
            availableArrows--;
            ShotArrows.Add(arrow);
            AudioPlayer.PlayOneShot(ShotFiredAudio);
        }
        else
        {
            ShotArrows.ForEach(arrow =>
                arrow.GetComponent<ArrowScript>().ReturnTo(this)
            );
        }
    }

    void Retrieve()
    {
        ShotArrows.Clear();
        availableArrows = Mathf.Min(availableArrows + 1, maxShots);
        onRetrieve?.Invoke();   
        AudioPlayer.PlayOneShot(ReloadingAudio);     
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Retrievable"))
        {
            Destroy(other.transform.parent.gameObject);
            Retrieve();
        }
    }
}
