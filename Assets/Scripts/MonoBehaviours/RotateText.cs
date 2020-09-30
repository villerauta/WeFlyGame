using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour
{
    private Transform cam;
    // Start is called before the first frame update
    void Start()


    {
        cam = FindObjectOfType<Basic_Follow_Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float targetAngle = cam.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f,targetAngle,0f);
    }
}
