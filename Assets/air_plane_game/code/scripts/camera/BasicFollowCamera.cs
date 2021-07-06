using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFollowCamera : MonoBehaviour
{
    #region Variables
    private Vector3 smoothVelocity;

    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 0.5f;

    protected float originalHeight;
    #endregion

    #region UntiyMethods
    void Start()
    {
        originalHeight = height;
    }

    void FixedUpdate()
    {
        if (target)
            handleCamera();
    }
    #  endregion

    #region CustomMethods
    protected virtual void handleCamera()
    {
        Vector3 wantedPosition = target.position + (-target.forward * distance) + (Vector3.up * height);
        transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref smoothVelocity, smoothSpeed);
        transform.LookAt(target);
    }
    #endregion
}
