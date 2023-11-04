using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyHealth : Health
{
    private Basic reference;
    public bool HitByPlayer;
    public bool died;

    public AudioSource myAud;
    public AudioClip myAudio;
    protected override void OnDeath()
    {
        if(!died)
        {
            myAud.PlayOneShot(myAudio);
        }
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
        
       // myAud = GetComponent<AudioSource>();
        
    }

    private IEnumerator Death()
    {
        if (!died)
        {
            died = true;
            GetComponent<SpriteRenderer>().color = Color.white;

            reference.rb.velocity = new Vector2(0, 0);
            reference.rb.AddForce(((transform.position - hitPoint).normalized + Vector3.up) * 12, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);

            if (HitByPlayer)
                ScoreAndTimer.Singleton.GainPoints(10);
            
            Destroy(gameObject);
        }


    }
}
