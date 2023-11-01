using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthCapsule : MonoBehaviour
{
    public GameObject AudioObject;
    public AudioClip myAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().GainHealth();
            GameObject clone = Instantiate(AudioObject);
            clone.transform.parent = null;
            clone.GetComponent<AudioObjectScript>().PlayAudio(myAudio);
            Destroy(gameObject);
        }
    }
}
