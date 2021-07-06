using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneCamera : BasicFollowCamera
{
    #region Variables
    public float minHeightFromGround = 2f;
    #endregion

    #region CustomMethods
    protected override void handleCamera()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
            if (checkHitWithGround(hit))
                height = originalHeight + (minHeightFromGround - hit.distance);
        base.handleCamera();
    }

    private bool checkHitWithGround(RaycastHit hit)
    {
        return hit.distance < minHeightFromGround && hit.transform.tag == "Ground";
    }
    #endregion
}
