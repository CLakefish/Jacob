/*111111111111111111111111111111111111111111111111111111111
 * 
 * Name        : Carson Lakefish
 * Date        : 9 / 18 / 2023
 * Description : Basic Enemy AI
 111111111111111111111111111111111111111111111111111111111*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Basic : StateMachine
{
    public Searching Searching;
    public Attacking Attacking;
    public Knockback Knockback;
    protected override State Initialize() { return Searching; }

    internal Rigidbody2D rb;
    internal MovementController player;
    internal SpriteRenderer sprite;
    internal Animator animator;

    [Header("Movement Parameters")]
    [SerializeField] internal float moveSpeed;

    [Header("Attack Parameters")]
    [SerializeField] internal float attackSpeed;
    [SerializeField] internal int attackDamage;
    [SerializeField] internal float attackTime, swingTime;

    [Header("Collisions")]
    public LayerMask layers;

    [Header("Attack Layers")]
    public LayerMask attackLayer;

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
        animator = GetComponent<Animator>();

        Searching = new(this);
        Attacking = new(this);
        Knockback = new(this);

        moveSpeed -= Random.Range(-2f, 2f);

        base.Start();
    }

    new void Update()
    {
        RaycastHit2D ground = Physics2D.BoxCast(rb.transform.position - new Vector3(0, sprite.bounds.extents.y - 0.01f, 0), new Vector2(sprite.bounds.size.x - 0.05f, 0.005f), 0, Vector2.down, 0.05f, layers);
        Grounded = ground.collider != null;

        if (currentState == Searching) sprite.flipX = rb.velocity.x < 0;

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

    public override void Enter()
    {
        Enemy.animator.SetBool("Swinging", false);
        Enemy.animator.SetBool("Hit", false);
        Enemy.animator.SetBool("Knockback", false);
    }

    public override void Update()
    {
        if (Vector2.Distance(Enemy.player.transform.position, Enemy.rb.transform.position) >= 2f)
        {
            Vector3 moveDir = Enemy.player.transform.position - Enemy.rb.transform.position;
            Enemy.rb.velocity = Vector2.SmoothDamp(Enemy.rb.velocity, new Vector2(moveDir.normalized.x * Enemy.moveSpeed, Enemy.rb.velocity.y), ref Enemy.currentVelocity, Enemy.Grounded ? 0.12f : 0.25f);
        }
        else if (Enemy.Grounded) Enemy.rb.velocity = Vector2.MoveTowards(Enemy.rb.velocity, new Vector2(0, 0), Time.deltaTime * 40);

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
    private bool hasHit = false;

    public Attacking(Basic enemy) : base(enemy) { }

    public override void Enter()
    {
        hasHit = false;
    }

    public override void Update()
    {
        if (Time.time >= Enemy.previousAttackTime + Enemy.attackTime)
        {
            Enemy.ChangeState(Enemy.Searching);
            return;
        }

        if (Time.time > Enemy.previousAttackTime + Enemy.swingTime && !hasHit)
        {
            Debug.Log("o");
            Enemy.animator.SetBool("Swinging", true);
            RaycastHit2D player = Physics2D.BoxCast(Enemy.transform.position, Vector3.one * 1.1f, 0, new Vector2(Mathf.Sign(Enemy.player.transform.position.x - Enemy.rb.transform.position.x), 0), 1.8f, Enemy.attackLayer);

            if (player.collider != null)
            {
                Enemy.animator.SetBool("Hit", true);
                hasHit = true;
                player.collider.GetComponent<PlayerHealth>().Hit(Enemy.attackDamage, Enemy.transform.position);
                player.collider.attachedRigidbody.AddForce((Enemy.player.transform.position - Enemy.transform.position).normalized * 5, ForceMode2D.Impulse);
            }

            return;
        }
    }

    public override void Exit()
    {
        hasHit = false;
        Enemy.previousAttackTime = Time.time;
    }
}

public class Knockback : BaseEnemyState
{
    private LayerMask enemyLayer;
    private Collider2D collider;
    private List<Collider2D> hit = new();

    public Knockback(Basic enemy) : base(enemy) { }

    public override void Enter()
    {
        Enemy.sprite.color = Color.red;
        Enemy.animator.SetBool("Knockback", true);
        enemyLayer = LayerMask.GetMask("Enemy");
        collider = Enemy.gameObject.GetComponent<Collider2D>();
    }

    public override void Update()
    {
        RaycastHit2D enemy = Physics2D.BoxCast(Enemy.rb.transform.position, Vector2.one, 0, Vector2.zero, 1f, enemyLayer);

        if (enemy.collider != null && enemy.collider != collider && !hit.Contains(enemy.collider))
        {
            hit.Add(enemy.collider);
            enemy.collider.GetComponent<Health>().Hit(1, Enemy.transform.position);
            enemy.collider.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}