using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
