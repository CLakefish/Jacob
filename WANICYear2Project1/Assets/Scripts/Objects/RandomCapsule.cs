using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCapsule : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RandomEvent(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void RandomEvent(GameObject Player)
    {
        int random = Random.Range(0, 100);
        if (random <= 44)
        {
            DoubleJump();
        }
        else if (random > 44 && random < 77)
        {
            SwingSize();
        }
        else
        {
            if(random < 50)
            {
                Health(Player);
            }
            else
            {
                Stamina(Player);
            }
        }
        Destroy(gameObject);
    }

    void Health(GameObject p)
    {
        p.GetComponent<PlayerHealth>().GainHealth();
    }
    void Stamina(GameObject p)
    {
        p.GetComponent<PlayerAttackController>().GainStamina();
    }
    void DoubleJump()
    {

    }

    void SwingSize()
    {

    }
}
