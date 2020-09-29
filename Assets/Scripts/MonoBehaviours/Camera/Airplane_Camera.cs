using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_Camera : Basic_Follow_Camera
{
    [Header("Airplane Camera Properties")]
    public float minHeigthFromGround = 2f;
    protected override void HandlePlaneCamera()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit)) {
            if(hit.distance < minHeigthFromGround && hit.transform.tag == "ground") {
                float wantedHeigth = origHeigt + minHeigthFromGround - hit.distance;
                height = wantedHeigth;
            }
        }
        base.HandlePlaneCamera();
    }
}
