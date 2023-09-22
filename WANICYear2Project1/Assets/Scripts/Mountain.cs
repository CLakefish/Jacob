using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothing;
    private Vector3 velocityRef;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, transform.position.y, 0), ref velocityRef, smoothing);
    }
}
