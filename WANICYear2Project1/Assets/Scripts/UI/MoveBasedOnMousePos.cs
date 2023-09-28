using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBasedOnMousePos : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float smoothingSpeed;
    private Vector3 currentPosition;

    void Update()
    {
        Vector3 newPos = transform.position + new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentPosition, smoothingSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1, 1), Mathf.Clamp(transform.position.y, -1, 1), transform.position.z);
    }
}
