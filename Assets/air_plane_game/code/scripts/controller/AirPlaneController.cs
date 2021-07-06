using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneCharacteristics))]
public class AirPlaneController : BaseRigidbodyController
{

    #region Variables
    private AirplaneKeyboardInput keyboardInput;
    private AirplaneXboxInput xboxInput;
    private AirPlaneCharacteristics characteristics;

    public Transform centerOfMass;
    public List<AirplaneEngine> engines = new List<AirplaneEngine>();
    public List<AirplaneWheels> wheels = new List<AirplaneWheels>();
    public List<AirplaneControlSurface> controlSurfaces = new List<AirplaneControlSurface>();
    #endregion

    #region UnityMethods
    public override void Start()
    {
        base.Start();
        keyboardInput = GetComponent<AirplaneKeyboardInput>();
        xboxInput = GetComponent<AirplaneXboxInput>();
        characteristics = GetComponent<AirPlaneCharacteristics>();

        if (rigidbody && centerOfMass)
            rigidbody.centerOfMass = centerOfMass.localPosition;

        if (wheels != null && wheels.Count > 0)
            wheels.ForEach(wheel => wheel.initWheel());

        if (characteristics && keyboardInput)
            characteristics.initCharacteristics(rigidbody, keyboardInput);
    }
    #endregion

    #region CustomMethods
    protected override void handlePhysics()
    {
        if (keyboardInput)
        {
            handleEngines();
            handleCharacteristics();
            handleControlSurfaces();
            handleWheels();
        }
    }

    private void handleEngines()
    {
        if (checkEngines()) 
            engines.ForEach(engine => rigidbody.AddForce(engine.calculateForce(keyboardInput.StickyThrottle)));
    }

    private void handleCharacteristics()
    {
        if (characteristics)
            characteristics.updateCharacteristics();
    }

    private void handleControlSurfaces()
    {
        if (checkControlSurfaces())
            controlSurfaces.ForEach(controlSurface => controlSurface.handleControlSurface(keyboardInput));
    }

    private void handleWheels()
    {
        if (checkWheels())
            wheels.ForEach(wheel => wheel.handleWheels(keyboardInput));
    }

    private bool checkEngines()
    {
        return engines != null && engines.Count > 0;
    }

    private bool checkControlSurfaces()
    {
        return controlSurfaces != null && controlSurfaces.Count > 0;
    }

    private bool checkWheels()
    {
        return wheels != null && wheels.Count > 0;
    }
    #endregion
}
