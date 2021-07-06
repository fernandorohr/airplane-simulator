using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    #region Constants
    private const int INVERTER = -1;
    #endregion

    #region CustomMethods
    public static float invertValue(float value)
    {
        return value * INVERTER;
    }
    #endregion
}
