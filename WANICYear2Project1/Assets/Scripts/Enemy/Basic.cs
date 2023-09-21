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
    public Attacking Attacking;
    protected override State Initialize() { return Searching; }

    internal Rigidbody2D rb;
    internal MovementController player;
    private SpriteRenderer sprite;

    [Header("Movement Parameters")]
    [SerializeField] internal float moveSpeed;

    [Header("Attack Parameters")]
    [SerializeField] internal float attackSpeed;
    [SerializeField] internal int attackDamage;
    [SerializeField] internal float attackTime, swingTime;

    [Header("Collisions")]
    public LayerMask layers;

    [Header("Fall Parameters")]
    [SerializeField] private float constantGravity;

    [Header("Attack Variables")]
    internal float previousAttackTime;

    internal Vector2 moveDir;
    internal Vector2 currentVelocity;

    public bool Grounded;

    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MovementController>();
        sprite = GetComponent<SpriteRenderer>();

        Searching = new(this);
        Attacking = new(this);

        moveSpeed -= Random.Range(-2f, 2f);

        base.Start();
    }

    new void Update()
    {
        RaycastHit2D ground = Physics2D.BoxCast(rb.transform.position - new Vector3(0, sprite.bounds.extents.y - 0.01f, 0), new Vector2(sprite.bounds.size.x - 0.05f, 0.005f), 0, Vector2.down, 0.05f, layers);
        Grounded = ground.collider != null;

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
        if (Vector2.Distance(Enemy.player.transform.position, Enemy.rb.transform.position) >= 2f)
        {
            Vector3 moveDir = Enemy.player.transform.position - Enemy.rb.transform.position;
            Enemy.rb.velocity = Vector2.SmoothDamp(Enemy.rb.velocity, new Vector2(moveDir.normalized.x * Enemy.moveSpeed, Enemy.rb.velocity.y), ref Enemy.currentVelocity, Enemy.Grounded ? 0.12f : 0.25f);
        }
        else if (Enemy.Grounded) Enemy.rb.velocity = new Vector2(0, 0);

        if (Vector2.Distance(Enemy.player.transform.position, Enemy.rb.transform.position) <= 2f && Time.time >= Enemy.previousAttackTime + Enemy.attackSpeed)
        {
            Enemy.previousAttackTime = Time.time;
            Enemy.ChangeState(Enemy.Attacking);
            return;
        }
    }

    public override void FixedUpdate()
    {
        if (Enemy.Grounded && Physics2D.Raycast(Enemy.rb.transform.position, new Vector2(Enemy.rb.velocity.normalized.x, 0), 1.5f, Enemy.layers))
        {
            Jump();
            return;
        }

        if (Enemy.Grounded && Enemy.rb.transform.position.y + 1 < Enemy.player.transform.position.y && Vector2.Distance(Enemy.rb.position, Enemy.player.transform.position) <= 4)
        {
            Jump();
            return;
        }
    }

    private void Jump()
    {
        Enemy.rb.velocity = new Vector2(Enemy.rb.velocity.x, 0);
        Enemy.rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }
}

public class Attacking : BaseEnemyState
{
    public Attacking(Basic enemy) : base(enemy) { }

    public override void Enter()
    {
        Enemy.rb.velocity = new Vector2(0, Enemy.rb.velocity.y);
    }

    public override void Update()
    {
        if (Time.time >= Enemy.previousAttackTime + Enemy.attackTime)
        {
            Enemy.ChangeState(Enemy.Searching);
            return;
        }

        if (Time.time > Enemy.previousAttackTime + Enemy.swingTime)
        {
            // Cast for player
            return;
        }
    }

    public override void Exit()
    {
        Enemy.previousAttackTime = Time.time;
    }
}