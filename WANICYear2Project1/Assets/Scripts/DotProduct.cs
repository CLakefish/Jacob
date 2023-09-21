using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProduct : MonoBehaviour
{
    [SerializeField] Transform otherObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dotProduct = Vector2.Dot(Vector2.right, (otherObject.position - transform.position).normalized);
        print(dotProduct);    
    }
}
