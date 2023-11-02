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

/*
Code review:
comments indicate with CR.

general advice: please add comments
*/

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
    [SerializeField] internal Transform glove;
    internal SpriteRenderer gloveSprite;

    [Header("Movement Parameters")]
    [SerializeField] internal float moveSpeed;

    [Header("Attack Parameters")]
    [SerializeField] internal float attackSpeed;
    [SerializeField] internal int attackDamage;
    [SerializeField] internal float attackTime, chargeTime, swingTime;

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
    internal int knockBackCount;

    public bool Grounded;
    public AudioClip MyClip;
    public AudioSource Source;

    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MovementController>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Source = GetComponent<AudioSource>();


        gloveSprite = glove.GetComponent<SpriteRenderer>();

        Searching = new(this);
        Attacking = new(this);
        Knockback = new(this);

        moveSpeed -= Random.Range(-2f, 2f);
        Attacking.myAudio = MyClip;
        Attacking.myAud = Source;
        base.Start();
    }

    new void Update()
    {
        RaycastHit2D ground = Physics2D.BoxCast(rb.transform.position - new Vector3(0, sprite.bounds.extents.y - 0.01f, 0), new Vector2(sprite.bounds.size.x - 0.05f, 0.005f), 0, Vector2.down, 0.05f, layers);
        Grounded = ground.collider != null;

        bool flip = player.transform.position.x - transform.position.x < 0;
        if (currentState == Searching)
        {
            sprite.flipX = flip;
            gloveSprite.flipX = flip;
        }

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
        Enemy.glove.transform.localPosition = Vector3.zero;
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
    public AudioSource myAud;
    public AudioClip myAudio;

    float currentChargeTime;

    public Attacking(Basic enemy) : base(enemy) { }
    
    public override void Enter()
    {
        currentChargeTime = 0;
        //instantly stops the enemy
        Enemy.rb.velocity = new Vector2(0, Enemy.rb.velocity.y);
    }

    public override void Update()
    {
        currentChargeTime = Mathf.MoveTowards(currentChargeTime, Enemy.chargeTime, Time.deltaTime);
        Enemy.glove.transform.localPosition = ((Vector2)Enemy.transform.position - (Vector2)Enemy.player.transform.position).normalized * (currentChargeTime / Enemy.chargeTime);

        // rotates the glove toward the player
        Vector3 look = Enemy.glove.transform.InverseTransformPoint(Enemy.player.transform.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Enemy.glove.transform.Rotate(0, 0, angle);

        // makes sure the glove is never flipped over
        Enemy.gloveSprite.flipX = false;
        Enemy.gloveSprite.flipY = Enemy.transform.position.x > Enemy.player.transform.position.x;

        if (currentChargeTime >= Enemy.chargeTime)
        {
            Attack();
            Enemy.ChangeState(Enemy.Searching);
            myAud.PlayOneShot(myAudio);
            Enemy.animator.SetTrigger("Attack");
        }
    }

    void Attack()
    {
        currentChargeTime = 0;

        Enemy.glove.rotation = Quaternion.identity;
        Enemy.gloveSprite.flipY = false;

        // CR: 2 magic numbers here
        RaycastHit2D player = Physics2D.BoxCast(Enemy.transform.position, Vector3.one * 1.1f, 0, new Vector2(Mathf.Sign(Enemy.player.transform.position.x - Enemy.rb.transform.position.x), 0), 1.8f, Enemy.attackLayer);

        if (player.collider != null && player.collider.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Hit(Enemy.attackDamage, Enemy.transform.position);
            // CR: magic number
            player.collider.attachedRigidbody.AddForce((Enemy.player.transform.position - Enemy.transform.position).normalized * 5, ForceMode2D.Impulse);
        }
        Enemy.StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        float timer = 0;
        while (timer < Enemy.swingTime)
        {
            Mathf.MoveTowards(timer, Enemy.swingTime, Time.deltaTime);
            Enemy.glove.transform.position = Vector2.Lerp(Enemy.glove.transform.position, (Enemy.player.transform.position - Enemy.transform.position).normalized, timer);
            yield return null;
        }
    }

    public override void Exit()
    {
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
        if (Enemy.knockBackCount <= 2) return;

        RaycastHit2D enemy = Physics2D.BoxCast(Enemy.rb.transform.position, Vector2.one, 0, Vector2.zero, 1f, enemyLayer);

        if (enemy.collider != null && enemy.collider != collider && !hit.Contains(enemy.collider))
        {
            Enemy.knockBackCount++;

            hit.Add(enemy.collider);

            enemy.collider.GetComponent<Basic>().knockBackCount = Enemy.knockBackCount;
            enemy.collider.GetComponent<Health>().Hit(1, Enemy.transform.position);
            enemy.collider.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}