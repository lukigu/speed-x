using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float followSpeed;
    public float rotateSpeed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
    }
}
