using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirPlaneCharacteristics : MonoBehaviour
{

    #region Constants
    private const float MPS_TO_KMH = 3.6f;
    private const int MINIMUM_SPEED = 0;
    private const float MINIMUM_MAGNITUDE = 1f;
    private const float BANKING_DEGREE = 90f;
    private const float BANKING_INTERPOLATION = 1f;
    #endregion

    #region Variables
    private new Rigidbody rigidbody;
    private AirplaneKeyboardInput keyboardInput;
    private float startDrag;
    private float startAngularDrag;
    private float maxMetersPerSecond;
    private float normalizedKilometersPerHour;
    private float angleOfAttack;
    private float pitchAngle;
    private float rollAngle;

    public float forwardSpeed;
    public float kilometersPerHour;
    public float rigidbodyLerpSpeed = 0.01f;
    public float maxKilometersPerHour = 170;
    public float maxLiftForce = 3500f;
    public float dragFactor = 0.01f;
    public float angularDragFactor = 0.5f;
    public float flapDragFactor = 0.1f;
    public float pitchSpeed = 500f;
    public float rollSpeed = 500f;
    public float yawSpeed = 500f;
    public float bankSpeed = 20f;
    public AnimationCurve liftCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    #endregion

    #region CustomMethods
    public void initCharacteristics(Rigidbody rigidbody, AirplaneKeyboardInput keyboardInput)
    {
        this.rigidbody = rigidbody;
        this.keyboardInput = keyboardInput;
        startDrag = this.rigidbody.drag;
        startAngularDrag = this.rigidbody.angularDrag;
        maxMetersPerSecond = maxKilometersPerHour / MPS_TO_KMH;
    }

    public void updateCharacteristics()
    {
        if (rigidbody)
        {
            calculateFowardSpeed();
            calculateLift();
            calculateDrag();
            handlePitch();
            handleRoll(); 
            handleYaw();
            handleBanking();
            handleRigidbodyTransform();
        }
    }

    private void calculateFowardSpeed()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        forwardSpeed = localVelocity.z;
        forwardSpeed = Mathf.Clamp(forwardSpeed, MINIMUM_SPEED, maxMetersPerSecond);

        calculateKilometersPerHour();
    }

    private void calculateKilometersPerHour()
    {
        kilometersPerHour = forwardSpeed * MPS_TO_KMH;
        kilometersPerHour = Mathf.Clamp(kilometersPerHour, MINIMUM_SPEED, maxKilometersPerHour);

        normalizedKilometersPerHour = Mathf.InverseLerp(0f, maxKilometersPerHour, kilometersPerHour);
    }

    private void calculateLift()
    {
        float liftForce = liftCurve.Evaluate(normalizedKilometersPerHour) * maxLiftForce;
        rigidbody.AddForce(liftForce * transform.up * calculateAngleOfAttack());
    }

    private float calculateAngleOfAttack()
    {
        angleOfAttack = Vector3.Dot(rigidbody.velocity.normalized, transform.forward);
        return angleOfAttack * angleOfAttack;
    }

    private void calculateDrag()
    {
        float flapDrag = keyboardInput.Flaps * flapDragFactor;
        float speedDrag = forwardSpeed * dragFactor;
        rigidbody.drag = startDrag + speedDrag + flapDrag;

        float angularSpeedDrag = forwardSpeed * angularDragFactor;
        rigidbody.angularDrag = startAngularDrag + angularSpeedDrag + flapDrag;
    }

    private void handlePitch()
    {
        Vector3 flatForward = transform.forward;
        flatForward.y = 0f;
        pitchAngle = Vector3.Angle(transform.forward, flatForward.normalized);

        rigidbody.AddTorque(keyboardInput.Pitch * pitchSpeed * transform.right * forwardSpeed);
    }

    private void handleRoll()
    {
        Vector3 flatRight = transform.right;
        flatRight.y = 0f;
        rollAngle = Vector3.SignedAngle(transform.right, flatRight.normalized, transform.forward);

        rigidbody.AddTorque(keyboardInput.Roll * rollSpeed * transform.forward * forwardSpeed);
    }

    private void handleYaw()
    {
        rigidbody.AddTorque(keyboardInput.Yaw * yawSpeed * transform.up * forwardSpeed);
    }

    private void handleBanking()
    {
        float bankSide = Mathf.InverseLerp(Util.invertValue(BANKING_DEGREE), BANKING_DEGREE, rollSpeed);
        float bankAmount = Mathf.Lerp(Util.invertValue(BANKING_INTERPOLATION), BANKING_INTERPOLATION, bankSide);

        rigidbody.AddTorque(bankAmount * bankSpeed * transform.up * forwardSpeed);
    }

    private void handleRigidbodyTransform()
    {
        if (rigidbody.velocity.magnitude > MINIMUM_MAGNITUDE)
        {
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity,
                                                transform.forward * forwardSpeed,
                                                angleOfAttack * forwardSpeed * Time.deltaTime * rigidbodyLerpSpeed);
            rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation,
                                                    Quaternion.LookRotation(rigidbody.velocity.normalized, transform.up), 
                                                    Time.deltaTime * rigidbodyLerpSpeed));
        }
    }
    #endregion
}
