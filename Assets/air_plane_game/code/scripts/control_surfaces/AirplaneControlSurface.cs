using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlSurfaceType
{
    Elevator,
    Rudder,
    Flap,
    Aileron
}

public class AirplaneControlSurface : MonoBehaviour
{

    #region Variables
    private float wantedAngle;

    public ControlSurfaceType type;
    public float maxAngle = 30f;
    public float smoothSpeed = 0.01f;
    public Transform controlSurfaceGraphic;
    public Vector3 axis;
    #endregion

    #region UnityMethods
    void Update()
    {
        if (controlSurfaceGraphic)
            rotateControlSurfaceGraphic();
    }
    #endregion

    #region CustomMethods
    public void handleControlSurface(AirplaneKeyboardInput keyboardInput)
    {
        float inputValue = 0f;
        switch (type)
        {
            case ControlSurfaceType.Rudder:
                inputValue = keyboardInput.Yaw;
                break;
            case ControlSurfaceType.Elevator:
                inputValue = keyboardInput.Pitch;
                break;
            case ControlSurfaceType.Flap:
                inputValue = keyboardInput.Flaps;
                break;
            case ControlSurfaceType.Aileron:
                inputValue = keyboardInput.Roll;
                break;
            default:
                break;
        }
        wantedAngle = maxAngle * inputValue;
    }

    private void rotateControlSurfaceGraphic()
    {
        Vector3 finalAngle = axis * wantedAngle;
        controlSurfaceGraphic.localRotation = Quaternion.Slerp(controlSurfaceGraphic.localRotation, 
                                                                Quaternion.Euler(finalAngle), 
                                                                Time.deltaTime * smoothSpeed);
    }
    #endregion
}
