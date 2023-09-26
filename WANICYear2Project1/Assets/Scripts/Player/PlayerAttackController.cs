using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] float groundAttackRadius;
    [SerializeField] float airAttackRadius;
    int direction;

    [SerializeField] SpriteRenderer groundAttackIndicator;

    Rigidbody2D rb;
    MovementController movementController;
    int maskEnemy;
    void Start()
    {
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody2D>();
        maskEnemy = LayerMask.GetMask("Enemy");
        groundAttackIndicator.size = 2 * groundAttackRadius * Vector2.one;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            direction = (int)Mathf.Sign(rb.velocity.x);
        }
    }

    void Attack()
    {
        if (movementController.currentState == movementController.Walking)
        {
            StartCoroutine(GroundAttack(0.2f));
        }
        else if (movementController.currentState == movementController.Jumping || movementController.currentState == movementController.Falling)
        {
            StartCoroutine(AirAttack(0.5f));
        }
    }

    IEnumerator GroundAttack(float duration)
    {
        groundAttackIndicator.flipX = direction == -1;
        groundAttackIndicator.enabled = true;

        float timer = duration;

        while (timer > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, groundAttackRadius, maskEnemy);
            for (int i = 0; i < hits.Length; i++)
            {
                int enemyDirection = (int)Mathf.Sign(hits[i].transform.position.x - transform.position.x);

                if (enemyDirection == direction && hits[i].attachedRigidbody != null)
                {
                    hits[i].GetComponent<Health>().Hit(1, transform.position);
                    print("Hit!");
                }
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        
        groundAttackIndicator.enabled = false;
    }
    IEnumerator AirAttack(float duration)
    {
        float timer = duration;

        while (timer > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, airAttackRadius, maskEnemy);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].attachedRigidbody != null)
                {
                    hits[i].GetComponent<Health>().Hit(1, transform.position);
                }
            }

            timer -= Time.deltaTime;

            yield return null;
        }

        // todo: add indicator enabling
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (Application.isPlaying)
        {
            float radius = movementController.currentState == movementController.Walking ? groundAttackRadius : airAttackRadius;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        Gizmos.color = Color.white;
        Gizmos.DrawRay(new Ray(transform.position, new Vector3(direction, 0)));
    }
}
