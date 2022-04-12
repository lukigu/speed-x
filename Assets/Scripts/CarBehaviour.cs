using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DriveType { RearWheelDrive, FrontWheelDrive, AllWheelDrive }
public class CarBehaviour : MonoBehaviour 
{
    [SerializeField] public float maxAngle;
    [SerializeField] public float maxTorque;
    [SerializeField] public float breakTorque;
    [SerializeField] GameObject wheelShape;

    [SerializeField] public float criticalSpeed;
    [SerializeField] public int stepBelow;
    [SerializeField] public int stepAbove;

    [SerializeField] DriveType driveType;
    WheelCollider[] m_Wheels;
    public float handBrake, torque;
    public float angle;

    public InputActionAsset inputActions;
    InputActionMap gameplayActionMap;
    InputAction handBrakeInputAction;
    InputAction steeringInputAction;
    InputAction accelerationInputAction;

    private void Awake()
    {
        gameplayActionMap = inputActions.FindActionMap("Gameplay");

        handBrakeInputAction = inputActions.FindAction("HandBrake");
        steeringInputAction = inputActions.FindAction("SteeringAngle");
        accelerationInputAction = inputActions.FindAction("Acceleration");

        handBrakeInputAction.performed += GetHandBrakeInput;
        handBrakeInputAction.canceled += GetHandBrakeInput;

        steeringInputAction.performed += GetAngleInput;
        steeringInputAction.canceled += GetAngleInput;

        accelerationInputAction.performed += GetTorqueInput;
        accelerationInputAction.canceled += GetTorqueInput;
    }

    void GetHandBrakeInput(InputAction.CallbackContext context)
    {
        handBrake = context.ReadValue<float>() * breakTorque;
    }

    void GetAngleInput(InputAction.CallbackContext context)
    {
        angle = context.ReadValue<float>() * maxAngle;
    }

    void GetTorqueInput(InputAction.CallbackContext context)
    {
        torque = context.ReadValue<float>() * maxTorque;
    }

    private void OnEnable()
    {
        handBrakeInputAction.Enable();
        steeringInputAction.Enable();
        accelerationInputAction.Enable();
    }

    private void OnDisable()
    {
        handBrakeInputAction.Disable();
        steeringInputAction.Disable();
        accelerationInputAction.Disable();
    }

    void Start()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();
        for(int i = 0; i < m_Wheels.Length; i++)
        {
            var wheel = m_Wheels[i];
            if(wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    private void Update()
    {
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbove);
        foreach(WheelCollider wheel in m_Wheels)
        {
            if(wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = angle;
            }
            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }
            if (wheel.transform.localPosition.z > 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if(wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform shapeTransform = wheel.transform.GetChild(0);
                if(wheel.name == "FL" || wheel.name == "RR") //|| wheel.name == ""
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.position = p;
                    shapeTransform.rotation = q;
                }
            }
        }
    }

    //[SerializeField]
    //public List<AxleInfo> axleInfos;
    //[SerializeField]
    //public float maxForwardMotorTorque;
    //[SerializeField]
    //public float maxBackwardMotorTorque;
    //[SerializeField]
    //public float breakForce;
    //[SerializeField]
    //public float maxSteeringAngle;

    //private Rigidbody rigidbody;

    //public void Start() {
    //    this.rigidbody = GetComponent<Rigidbody>();
    //}

    //public void FixedUpdate() {
    //    float verticalAxis = Input.GetAxis("Vertical");
    //    float motor;
    //    float brForce = 0;
    //    if (verticalAxis < 0) {
    //        motor = maxBackwardMotorTorque * verticalAxis;
    //    }else {
    //        motor = maxForwardMotorTorque * verticalAxis;
    //    }
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        brForce = this.breakForce;
    //    }
    //    float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

    //    foreach (AxleInfo axleInfo in this.axleInfos) {

    //        if (axleInfo.motor)
    //        {
    //            axleInfo.leftWheel.motorTorque = motor;
    //            axleInfo.rightWheel.motorTorque = motor;
    //        }
    //        if (axleInfo.steering) {
    //            axleInfo.leftWheel.steerAngle = steering;
    //            axleInfo.rightWheel.steerAngle = steering;
    //        }
    //        if (axleInfo.brake)
    //        {
    //            axleInfo.leftWheel.brakeTorque = brForce;
    //            axleInfo.rightWheel.brakeTorque = brForce;
    //        }
    //    }
    //}

    //public void Update()
    //{
    //}

    //// called when we press down the accelerate input
    //public void OnAccelerateInput(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Performed)
    //        accelerateInput = true;
    //    else
    //        accelerateInput = false;
    //}

    //// called when we modify the turn input
    //public void OnTurnInput(InputAction.CallbackContext context)
    //{
    //    turnInput = context.ReadValue<float>();
    //}
}

//[System.Serializable]
//public class AxleInfo {
//    public WheelCollider leftWheel;
//    public WheelCollider rightWheel;
//    public bool motor;
//    public bool steering;
//    public bool brake;
//}