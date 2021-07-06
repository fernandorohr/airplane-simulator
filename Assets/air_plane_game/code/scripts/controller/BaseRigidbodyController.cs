using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class BaseRigidbodyController : MonoBehaviour
{

    #region Variables
    protected new Rigidbody rigidbody;
    protected AudioSource audioSource;
    #endregion

    #region UnityMethods
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
            audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (rigidbody)
            handlePhysics();
    }
    #endregion

    #region CustomMethods
    protected virtual void handlePhysics() { }

    #endregion
}
