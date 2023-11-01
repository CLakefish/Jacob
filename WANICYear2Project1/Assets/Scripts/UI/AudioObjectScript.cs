using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectScript : MonoBehaviour
{
    public AudioSource myAud;
    // Start is called before the first frame update
    void Start()
    {
        myAud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayAudio(AudioClip clip)
    {
        myAud.PlayOneShot(clip);
    }
}
