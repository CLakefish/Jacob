using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateScoreKeeper : MonoBehaviour
{
    public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        if(!GameObject.FindGameObjectWithTag("ScoreKeeper"))
        {
            Instantiate(Prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
