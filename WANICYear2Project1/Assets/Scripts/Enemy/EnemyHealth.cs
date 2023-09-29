using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyHealth : Health
{
    private Basic reference;
    public bool HitByPlayer;
    public bool died;

    protected override void OnDeath()
    {
        reference.ChangeState(reference.Knockback);
        StartCoroutine(Death());
    }

    protected override void OnHit()
    {
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

            reference.rb.AddForce(((transform.position - hitPoint).normalized + Vector3.up) * 4, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);

            if (HitByPlayer)
                ScoreAndTimer.Singleton.GainPoints(10);
            else
                ScoreAndTimer.Singleton.multiplyChain();

            Destroy(gameObject);
        }


    }
}
