using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetObj;
    private float distToTarget = 3.0f;
    public float smoothness = 3.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetObj.position - transform.forward * distToTarget, smoothness * Time.deltaTime);
    }
}
