using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaCapsule : MonoBehaviour
{

    public AudioClip myAudio;
    public GameObject AudioObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerAttackController>().GainStamina();
            GameObject clone = Instantiate(AudioObject);
            clone.transform.parent = null;
            clone.GetComponent<AudioObjectScript>().PlayAudio(myAudio);
            Destroy(gameObject);
        }

    }
}
