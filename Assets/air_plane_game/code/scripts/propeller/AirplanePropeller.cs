using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplanePropeller : MonoBehaviour
{

    #region Constants
    private const int MAX_DEGREES = 360;
    private const int MAX_SECONDS = 60;
    #endregion

    #region Variables
    public float minQuadRpm = 600f;
    public float minTextureSwap = 1200f;

    public GameObject mainPropeller;
    public GameObject blurredPropeller;

    public Material blurredPropellerMaterial;
    public Texture2D firstBlur;
    public Texture2D secondBlur;
    #endregion

    #region UnityMethods
    void Start()
    {
        setNoBlur();
    }
    #endregion

    #region CustomMethods
    public void handlePropeller(float rpm)
    {
        transform.Rotate(Vector3.forward, ((rpm * MAX_DEGREES) / MAX_SECONDS) * Time.deltaTime);

        if (mainPropeller && blurredPropeller)
            swapPropellers(rpm);
    }

    private void swapPropellers(float rpm)
    {
        if (rpm > minQuadRpm)
        {
            setBlur();
            if (checkMaterialAndTextures())
                swapBlurs(rpm);
        }
        else
            setNoBlur();
    }

    private void setBlur()
    {
        mainPropeller.SetActive(false);
        blurredPropeller.SetActive(true);
    }

    private void setNoBlur()
    {
        mainPropeller.SetActive(true);
        blurredPropeller.SetActive(false);
    }

    private void swapBlurs(float rpm)
    {
        if (rpm > minTextureSwap)
            blurredPropellerMaterial.SetTexture("_MainText", secondBlur);
        else
            blurredPropellerMaterial.SetTexture("_MainText", firstBlur);
    }

    private bool checkMaterialAndTextures()
    {
        return blurredPropellerMaterial && firstBlur && secondBlur;
    }
    #endregion
}
