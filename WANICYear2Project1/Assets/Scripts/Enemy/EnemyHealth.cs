using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyHealth : Health
{
    private Basic reference;
    public bool HitByPlayer;
    public bool died;

    public GameObject Coin;
    public int MaxCoinsPerDeath;
    public int CoinSprayForce;
    protected override void OnDeath()
    {
        reference.ChangeState(reference.Knockback);
        StartCoroutine(Death());
    }

    protected override void OnHit()
    {
        reference.ChangeState(reference.Knockback);
        return;
    }

    private void Start()
    {
        reference = GetComponent<Basic>();
        HitByPlayer = false;
        died = false;
    }

    private IEnumerator Death()
    {
        if (!died)
        {
            died = true;
            GetComponent<SpriteRenderer>().color = Color.white;

            for (int i = 0; i < Random.Range(1, MaxCoinsPerDeath); i++)
            {
                GameObject CoinClone = Instantiate(Coin, gameObject.transform);
                CoinClone.transform.parent = null;
                CoinClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(0, 10)) * CoinSprayForce);
            }

            reference.rb.velocity = new Vector2(0, 0);
            reference.rb.AddForce(((transform.position - hitPoint).normalized + Vector3.up) * 12, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);

            if (HitByPlayer)
                ScoreAndTimer.Singleton.GainPoints(10);
            
            Destroy(gameObject);
        }


    }
}
