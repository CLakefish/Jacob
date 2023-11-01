using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimetoDie : MonoBehaviour
{
    public float TimeToDie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( TimeToDie > 0 )
        {
            TimeToDie -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
