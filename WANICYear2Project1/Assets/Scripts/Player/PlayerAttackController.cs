using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Sword Radius")]
    [SerializeField] private float groundAttackRadius;
    [SerializeField] private float airAttackRadius;
    private int direction;
    private int swingCount;

    //[SerializeField] SpriteRenderer groundAttackIndicator;
    //[SerializeField] SpriteRenderer airAttackIndicator;
    [Header("References")]
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject hitParticles;
    private GameObject swordObj;

    Rigidbody2D rb;
    MovementController movementController;
    int maskEnemy;

    [Tooltip("slider for Stamina")] public Slider AttackSlider;
    private float SpentAttack;
    [Tooltip("time until Stamina increases faster")] public int AttackTime;
    [Tooltip("MaxStamina")] public int MaxStamina;
    [Tooltip("stamina")] public float Stamina;
    [Tooltip("stamina gained per second")] public float staminaGain;
    [Tooltip("Main attack Stamina Loss")] public int MainAttackLoss;
    [Tooltip("Jump attack Stamina loss")] public int HeavyAttackLoss;
    [Tooltip("lower threshold for attacking")] public int attackthreshold;
    void Start()
    {
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody2D>();
        maskEnemy = LayerMask.GetMask("Enemy");
        //groundAttackIndicator.size = 2 * groundAttackRadius * Vector2.one;
        //airAttackIndicator.size = 2 * airAttackRadius * Vector2.one;
        AttackSlider.maxValue = MaxStamina;
        AttackSlider.value = MaxStamina;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Stamina >= attackthreshold)
        {
            Attack();
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            direction = (int)Mathf.Sign(rb.velocity.x);
        }

        if(Stamina < MaxStamina) //if you dont have full stamina
        {
            if(SpentAttack > 0)
            {
                Stamina += staminaGain;
                SpentAttack -= Time.deltaTime;
            }
            else
            {
                Stamina += staminaGain * 2;
                SpentAttack = 0; //reseting spent attack just in case
            }
        }
        else
        {
            Stamina = MaxStamina;
        }

        AttackSlider.value = Stamina;
    }

    void Attack()
    {
        if (swordObj != null) Destroy(swordObj);

        swordObj = Instantiate(sword, transform, false);
        swingCount++;
        swingCount %= 2;

        SpriteRenderer swordSprite = swordObj.GetComponent<SpriteRenderer>();
        swordSprite.flipX = direction != 1;
        swordSprite.flipY = swingCount == 1;

        if (movementController.currentState == movementController.Walking && Stamina > attackthreshold)
        {
            StartCoroutine(GroundAttack(0.2f));
            StaminaLose(MainAttackLoss);
        }
        else if (movementController.currentState == movementController.Jumping || movementController.currentState == movementController.Falling && Stamina > attackthreshold)
        {
            StaminaLose(HeavyAttackLoss);
            StartCoroutine(AirAttack(0.5f));
        }
    }
    private void StaminaLose(int Loss)
    {
        Stamina -= Loss;
        SpentAttack = AttackTime;
    }
    public void StaminaGain()
    {
        MaxStamina += 25;
        AttackSlider.maxValue = MaxStamina;
    }
    IEnumerator GroundAttack(float duration)
    {
        //groundAttackIndicator.flipX = direction == 1;
        //groundAttackIndicator.enabled = true;
        Destroy(swordObj, duration);

        float timer = duration;

        while (timer > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, groundAttackRadius, maskEnemy);
            
            for (int i = 0; i < hits.Length; i++)
            {
                int enemyDirection = (int)Mathf.Sign(hits[i].transform.position.x - transform.position.x);

                if ((enemyDirection == direction) && hits[i].attachedRigidbody != null)
                {
                    EnemyHealth h = hits[i].GetComponent<EnemyHealth>();

                    if (!h.died) {
                        Instantiate(hitParticles, hits[i].attachedRigidbody.transform.position, Quaternion.identity);
                        StartCoroutine(HitVFX());
                    }

                    h.Hit(1, transform.position);
                    h.HitByPlayer = true;
                }
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        
        //groundAttackIndicator.enabled = false;
    }
    IEnumerator AirAttack(float duration)
    {
        //airAttackIndicator.enabled = true;
        swordObj.GetComponent<Animator>().SetBool("Air", true);
        Destroy(swordObj, duration);

        float timer = duration;

        while (timer > 0)
        {

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, airAttackRadius, maskEnemy);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].attachedRigidbody != null)
                {
                    EnemyHealth h = hits[i].GetComponent<EnemyHealth>();

                    if (!h.died) {
                        Instantiate(hitParticles, hits[i].attachedRigidbody.transform.position, Quaternion.identity);
                        StartCoroutine(HitVFX());
                    } 

                    h.Hit(1, transform.position);
                    h.HitByPlayer = true;
                }
            }

            timer -= Time.deltaTime;

            yield return null;
        }

        //airAttackIndicator.enabled = false;
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

    private IEnumerator HitVFX() {

        for (int i = 0; i < 5; i++)
        {
            Camera.main.transform.eulerAngles += new Vector3(0, 0, Random.insideUnitCircle.y);
            yield return new WaitForSeconds(0.001f);
        }

        float time = 0.0000005f;

        Time.timeScale = 0.00001f;

        yield return new WaitForSeconds(time);

        Time.timeScale = 1;
    }

    public void GainStamina()
    {
        MaxStamina++;
        Stamina = MaxStamina;
        AttackSlider.maxValue++;
        AttackSlider.value = MaxStamina;
    }
}
