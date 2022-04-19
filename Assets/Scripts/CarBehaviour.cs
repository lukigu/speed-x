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
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            var wheel = m_Wheels[i];
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    private void Update()
    {
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbove);
        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
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
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform shapeTransform = wheel.transform.GetChild(0);
                if (wheel.name == "FL" || wheel.name == "RR") //|| wheel.name == ""
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
}