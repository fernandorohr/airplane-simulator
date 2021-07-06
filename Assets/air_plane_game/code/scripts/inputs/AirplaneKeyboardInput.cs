using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneKeyboardInput : MonoBehaviour
{
    #region Constants
    protected const int BRAKE_ON = 1;
    protected const int BRAKE_OFF = 0;
    private const KeyCode ADD_FLAPS = KeyCode.F;
    private const KeyCode REMOVE_FLAPS = KeyCode.V;
    protected const int FLAP_MAXIMUM = 2;
    protected const int FLAP_MINIMUM = 0;
    #endregion

    #region Variables
    public float pitch = 0f;
    public float roll = 0f;
    public float yaw = 0f;
    public float throttle = 0f;
    public float throttleSpeed = 0.1f;
    public float stickyThrottle = 0;
    public int flaps = 0;
    public int brakes = 0;
    #endregion

    #region Getters
    public float Pitch
    {
        get { return pitch; }
    }
    public float Roll
    {
        get { return roll; }
    }
    public float Yaw
    {
        get { return yaw; }
    }
    public float Throttle
    {
        get { return throttle; }
    }
    public float StickyThrottle
    {
        get { return stickyThrottle; }
    }
    public float Flaps
    {
        get { return flaps; }
    }
    public float Brakes
    {
        get { return brakes; }
    }
    #endregion

    #region UnityMethods
    void Start()
    {
        
    }

    void Update()
    {
        handleInput();
    }
    #endregion

    #region CustomMethods
    protected virtual void handleInput()
    {
        pitch = Input.GetAxis("Pitch");
        roll = Input.GetAxis("Roll");
        yaw = Input.GetAxis("Yaw");
        throttle = Input.GetAxis("Throtle");
        brakes = Input.GetKey(KeyCode.Space) ? BRAKE_ON : BRAKE_OFF;
        flapsInput();
        stickyThrottleControl();
    }

    protected virtual void flapsInput()
    {
        if (Input.GetKeyDown(ADD_FLAPS))
            flaps++;
        if (Input.GetKeyDown(REMOVE_FLAPS))
            flaps--;
        flaps = Mathf.Clamp(flaps, FLAP_MINIMUM, FLAP_MAXIMUM);
    }

    protected void stickyThrottleControl()
    {
        stickyThrottle += throttle * throttleSpeed * Time.deltaTime;
        stickyThrottle = Mathf.Clamp01(stickyThrottle);
    }
    #endregion
}
