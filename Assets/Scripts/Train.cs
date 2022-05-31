using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float speed;
    private Vector3 startPosition;
    public float frequency = 5f;
    public float magnitude = 5f;
    public float offset = 5f;
    public int function = 0;

    Vector3 bar;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if(function == 0)
            transform.position = startPosition + transform.forward * Mathf.Sin(Time.time * frequency + offset) * magnitude;

        if(function == 1)
            transform.position = startPosition + transform.forward * Mathf.Sin(Time.time * frequency + offset) * magnitude * -1;

       
    }
}
