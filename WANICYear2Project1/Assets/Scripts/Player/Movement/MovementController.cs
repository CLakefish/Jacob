/*111111111111111111111111111111111111111111111111111111111
 * 
 * Name        : Carson Lakefish
 * Date        : 9 / 14 / 2023
 * Description : Main Player States
 111111111111111111111111111111111111111111111111111111111*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : StateMachine
{
    [Header("States")]
    internal Walking Walking;
    internal Jumping Jumping;
    internal Falling Falling;
    protected override State Initialize() { return Walking; }

    [Header("Collisions")]
    // CR: changed "public LayerMask layers" to this, since we need to follow the convention
    [SerializeField] private LayerMask layers;

    [Header("References")]
    public static MovementController Instance;
    Camera playerCamera;
    [SerializeField] private SpriteRenderer visual;
    internal Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundAcceleration, groundDeceleration, airAcceleration, airDeceleration;

    [Header("Jump Parameters")]
    [SerializeField] internal float jumpForce;
    [SerializeField] internal float jumpStartBoost, jumpCancelSpeed, jumpBuffer, coyoteTime;

    [Header("Jump Apex Parameters")]
    [SerializeField] private float jumpApexBoost;
    [SerializeField] private float jumpApexVerticalBoost;
    [SerializeField] private Vector2 jumpApexThreshold;

    [Header("Fall Parameters")]
    [SerializeField] private float constantGravity;

    [Header("Inputs")]
    internal Vector2 input;
    internal bool inputting;

    [Header("Collision Variables")]
    internal bool Grounded = false;

    [Header("Movement Variables")]
    private Vector2 currentVelocity;
    private Vector3 camVelocity;

    [Header("Jump Variables")]
    internal Coroutine jumpStop;
    internal float jumpB;

    private void Reload()
    {
        Walking = new(this);
        Jumping = new(this);
        Falling = new(this);

        Instance = this;
    }

    new void Start()
    {
        Reload();

        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerCamera = FindObjectOfType<Camera>();

        base.Start();
    }

    const float GROUNDCAST_X_IN = 0.02f;
    const float GROUNDCAST_Y_OUT = 0.02f;

    new void Update()
    {
        // Improve Camera Behaviour

        //CR: replace these magic numbers please
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, new Vector3(rb.transform.position.x, rb.transform.position.y + 0.6f, -10), ref camVelocity, 0.068f);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * 4);

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputting = input != Vector2.zero;

        // calculate grounded
        Vector2 overlapCenter = col.bounds.center - new Vector3(0, (col.bounds.size.y / 2) - (GROUNDCAST_Y_OUT / 2));
        Vector2 overlapSize = new Vector2(col.bounds.size.x - GROUNDCAST_X_IN, GROUNDCAST_Y_OUT);
        Collider2D ground = Physics2D.OverlapBox(overlapCenter, overlapSize, 0, layers);
        Grounded = ground != null;

        float velocityChange = Grounded ? inputting ? groundAcceleration : groundDeceleration : inputting ? airAcceleration : airDeceleration;
        Vector2 moveDir = new Vector2(input.x * moveSpeed, rb.velocity.y);

        rb.velocity = Vector2.SmoothDamp(rb.velocity, moveDir, ref currentVelocity, velocityChange);

        visual.flipX = rb.velocity.x != 0 && Mathf.Sign(rb.velocity.x) == 1;

        base.Update();
    }

    new void FixedUpdate()
    {
        if (!Grounded) rb.AddForce(new(0, constantGravity), ForceMode2D.Impulse);

        jumpB -= Time.deltaTime;

        if (currentState == Jumping || (currentState == Falling && previousState == Jumping))
        {
            if (Input.GetKey(KeyCode.Space) && rb.velocity.y <= jumpApexThreshold.y && rb.velocity.y > jumpApexThreshold.x)
            {
                rb.AddForce(new Vector2(inputting ? input.x * jumpApexBoost : 0, jumpApexVerticalBoost), ForceMode2D.Impulse);
            }
        }

        base.FixedUpdate();
    }

    #region Coroutines

    internal IEnumerator JumpCancel()
    {
        while (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.SmoothDamp(rb.velocity.y, 0, ref currentVelocity.y, jumpCancelSpeed));
            yield return null;
        }
    }

    #endregion
}

#region States

public class PlayerState : State
{
    public MovementController Player;
    public PlayerState(MovementController player) => Player = player;
}

public class Walking : PlayerState
{
    public Walking(MovementController player) : base(player) {  }

    public override void Update()
    {
        if (Player.Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Player.jumpB > 0) {
                // Debug.Log("o");
                Player.ChangeState(Player.Jumping);
                return;
            }

            return;
        } 

        Player.ChangeState(Player.Falling);
    }

}

public class Jumping : PlayerState
{
    public Jumping(MovementController player) : base(player) { }

    public override void Enter() {
        if (Player.jumpStop != null) Player.StopCoroutine(Player.jumpStop);

        Player.jumpB = 0;

        Player.rb.velocity = new Vector2(Player.rb.velocity.x, Player.jumpStartBoost);
        Player.rb.AddForce(new Vector2(0, Player.jumpForce), ForceMode2D.Impulse);
    }

    public override void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            Player.jumpStop = Player.StartCoroutine(Player.JumpCancel());
            Player.ChangeState(Player.Falling);
            return;
        }

        if (Player.rb.velocity.y < 0) Player.ChangeState(Player.Falling);
    }
}

public class Falling : PlayerState
{
    public Falling(MovementController player) : base(player) { }

    public override void Update()
    {
        if (Player.Grounded)
        {
            Player.ChangeState(Player.Walking);
            return;
        }    

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Player.previousState == Player.Walking && Player.stateDuration < Player.coyoteTime)
            {
                Player.ChangeState(Player.Jumping);
                return;
            }

            Player.jumpB = Player.jumpBuffer;
        }
    }

    public override void Exit()
    {
        if (Player.jumpStop != null) Player.StopCoroutine(Player.jumpStop);
    }
}

#endregion
