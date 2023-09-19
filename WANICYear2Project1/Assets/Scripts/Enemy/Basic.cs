/*111111111111111111111111111111111111111111111111111111111
 * 
 * Name        : Carson Lakefish
 * Date        : 9 / 18 / 2023
 * Description : Basic Enemy AI
 111111111111111111111111111111111111111111111111111111111*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Basic : StateMachine
{
    public Searching Searching;
    protected override State Initialize() { return Searching; }

    internal Rigidbody2D rb;
    internal MovementController player;
    private SpriteRenderer sprite;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed;

    [Header("Collisions")]
    public LayerMask layers;

    [Header("Fall Parameters")]
    [SerializeField] private float constantGravity;

    internal Vector2 moveDir;
    private Vector2 currentVelocity;

    public bool Grounded;

    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MovementController>();
        sprite = GetComponent<SpriteRenderer>();

        Searching = new(this);

        base.Start();
    }

    new void Update()
    {

        RaycastHit2D ground = Physics2D.BoxCast(rb.transform.position - new Vector3(0, sprite.bounds.extents.y - 0.01f, 0), new Vector2(sprite.bounds.size.x - 0.05f, 0.005f), 0, Vector2.down, 0.05f, layers);
        Grounded = ground.collider != null;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(moveDir.normalized.x * moveSpeed, rb.velocity.y), ref currentVelocity, Grounded ? 0.12f : 0.25f);

        base.Update();
    }

    new void FixedUpdate()
    {
        if (!Grounded) rb.AddForce(new(0, constantGravity), ForceMode2D.Impulse);

        base.FixedUpdate();
    }
}

public class BaseEnemyState : State
{
    public Basic Enemy;
    public BaseEnemyState(Basic enemy) => Enemy = enemy;
}

public class Searching : BaseEnemyState
{
    public Searching(Basic enemy) : base(enemy) { }

    public override void Update()
    {
        Enemy.moveDir = Enemy.player.transform.position - Enemy.rb.transform.position;

        if (Vector2.Distance(Enemy.player.transform.position, Enemy.rb.transform.position) <= 1.5f) return;
    }

}