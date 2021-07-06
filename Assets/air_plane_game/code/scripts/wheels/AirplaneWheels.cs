using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class AirplaneWheels : MonoBehaviour
{

    #region Constants
    private const float MOTOR_TORQUE = 0.000000000000001f;
    private const float MINIMUM_BRAKE_FORCE = 0.1f;
    private const float BRAKE_OFF = 0f;
    #endregion

    #region Variables
    private WheelCollider wheelCollider;
    private Vector3 worldPosition;
    private Quaternion worldRotation;
    private float finalBrakingForce;
    private float finalSteeringAngle;

    public Transform wheelGraphic;
    public bool isBraking = false;
    public float brakingForce = 5f;
    public bool isSteering = false;
    public float steeringAngle = 30f;
    public float smoothSteeringSpeed = 2f;
    #endregion

    #region UnityMethods
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }
    #endregion

    #region CustomMethods
    public void initWheel()
    {
        if (wheelCollider)
            wheelCollider.motorTorque = MOTOR_TORQUE;
    }

    public void handleWheels(AirplaneKeyboardInput keyboardInput)
    {
        if (wheelCollider)
        {
            handlePositionAndRotation();
            handleBrakes(keyboardInput);
            handleSteering(keyboardInput);
        }
    }

    private void handlePositionAndRotation()
    {
        wheelCollider.GetWorldPose(out worldPosition, out worldRotation);
        if (wheelGraphic)
        {
            wheelGraphic.position = worldPosition;
            wheelGraphic.rotation = worldRotation;
        }
    }

    private void handleBrakes(AirplaneKeyboardInput keyboardInput)
    {
        if (checkForBraking(keyboardInput))
        {
            finalBrakingForce = Mathf.Lerp(finalBrakingForce, keyboardInput.Brakes * brakingForce, Time.deltaTime);
            wheelCollider.brakeTorque = finalBrakingForce;
        }
        else
        {
            finalBrakingForce = BRAKE_OFF;
            wheelCollider.motorTorque = MOTOR_TORQUE;
            wheelCollider.brakeTorque = BRAKE_OFF;
        }
    }

    private bool checkForBraking(AirplaneKeyboardInput keyboardInput)
    {
        return keyboardInput.Brakes > MINIMUM_BRAKE_FORCE && isBraking;
    }

    private void handleSteering(AirplaneKeyboardInput keyboardInput)
    {
        if (isSteering)
        {
            finalSteeringAngle = Mathf.Lerp(finalSteeringAngle, 
                                            Util.invertValue(keyboardInput.Yaw) * steeringAngle, 
                                            Time.deltaTime * smoothSteeringSpeed);
            wheelCollider.steerAngle = finalSteeringAngle;
        }            
    }
    #endregion
}
