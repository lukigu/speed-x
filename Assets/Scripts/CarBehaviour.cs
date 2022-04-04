using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour {
    [SerializeField]
    public List<AxleInfo> axleInfos;
    [SerializeField]
    public float maxForwardMotorTorque;
    [SerializeField]
    public float maxBackwardMotorTorque;
    [SerializeField]
    public float breakForce;
    [SerializeField]
    public float maxSteeringAngle;

    private Rigidbody rigidbody;

    public void Start() {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate() {
        float verticalAxis = Input.GetAxis("Vertical");
        float motor;
        float brForce = 0;
        if (verticalAxis < 0) {
            motor = maxBackwardMotorTorque * verticalAxis;
        }else {
            motor = maxForwardMotorTorque * verticalAxis;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            brForce = this.breakForce;
        }
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in this.axleInfos) {

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.brake)
            {
                axleInfo.leftWheel.brakeTorque = brForce;
                axleInfo.rightWheel.brakeTorque = brForce;
            }
        }
    }

    public void Update()
    {
    }
}

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool brake;
}