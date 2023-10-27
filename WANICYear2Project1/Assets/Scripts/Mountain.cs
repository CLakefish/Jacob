using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothing;
    private Vector3 startpos;
    private Vector3 velocityRef;

    private void Start()
    {
        startpos = transform.position;
    }

    void Update()
    {
        float distance = (target.transform.position.x - startpos.x) * 0.8f;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(startpos.x + distance, transform.position.y, 0), ref velocityRef, smoothing);
    }
}
