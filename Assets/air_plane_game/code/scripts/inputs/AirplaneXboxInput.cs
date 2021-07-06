using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneXboxInput : AirplaneKeyboardInput
{
    #region Variables
    private const string ADD_FLAPS = "XboxAddFlaps";
    private const string REMOVE_FLAPS = "XboxRemoveFlaps";
    #endregion

    #region CustomMethods
    protected override void handleInput()
    {
        pitch = Input.GetAxis("XboxPitch");
        roll = Input.GetAxis("XboxRoll");
        yaw = Input.GetAxis("XboxYaw");
        throttle = Input.GetAxis("XboxThrotle");
        brakes = Input.GetButton("XboxBrake") ? BRAKE_ON : BRAKE_OFF;
        flapsInput();
    }

    protected override void flapsInput()
    {
        if (Input.GetButtonDown(ADD_FLAPS))
            flaps++;
        if (Input.GetButtonDown(REMOVE_FLAPS))
            flaps--;
        flaps = Mathf.Clamp(flaps, FLAP_MINIMUM, FLAP_MAXIMUM);
    }
    #endregion
}
