using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Follow_Camera : MonoBehaviour
{
    [Header("Basic Follow Camera Properties")]
    public Transform target;
    public float distance = 5f;
    public float characterDistanceFactor = 0.7f;
    public float height = 2f;
    public float smoothSpeed = 0.05f;
    public float freeLookSmooth = 0.01f;
    public float distanceToTarget = 0f;


    [Header("Camera Rotation Speeds")]
    public float xRotSpeed = 0.02f;
    public float yRotSpeed = 0.02f;
    public float planeXRotSpeed = 0.1f;
    public float planeYRotSpeed = 0.1f;


    [Header("Camera Rotation Limits")]
    public float maxYRot = 45f;
    public float minYRot = 5f;

    public float maxYRot3rd = 30f;
    public float minYRot3rd = 10f;

    //Rotations
    private float xRotation;
    private float xSin;
    private float xCos;

    private float yRotation;
    private Vector3 yMove;
    private Vector3 xMove;

    protected float origHeigt;
    protected float origDist;

    private bool still = false;

    private Vector3 smoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        origHeigt = height;
        origDist = distance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!still) {
            if (target && target.tag == "plane") {
                HandlePlaneCamera();  
            }
        }
    }

    void Update() 
    {
        if(!still) {
            if (target && target.tag == "player") {
                HandleCharacterCamera();
            }
        }
    }

    protected virtual void HandlePlaneCamera() {
        Vector3 wantedPosition = target.position +(-target.forward * distance) + (Vector3.up * height);
        

        if (Input.GetMouseButton(1)) {

            PlaneFreeLookCamera();

        } else {

            FollowCamera(wantedPosition);
        }

        transform.LookAt(target);
    }

    void HandleCharacterCamera() {
        
        Vector3 wantedPosition = target.position +(-target.forward * distance) + (Vector3.up * height);
        transform.LookAt(target);

        CharacterFollow(wantedPosition);
        
        
    }

    void PlaneFreeLookCamera() {
        //X-Axis Rotation
        xRotation = xRotation + (-Input.GetAxis("Mouse X") * planeXRotSpeed);
        xSin = Mathf.Sin(xRotation);
        xCos = Mathf.Cos(xRotation);

        //Y-Axis Rotation
        float maxYRads = maxYRot*Mathf.Deg2Rad;
        float maxYDist = maxYRads * distanceToTarget;
        yRotation = yRotation + Input.GetAxis("Mouse Y") * planeYRotSpeed;
        yRotation = Mathf.Clamp(yRotation, -maxYRads, maxYRads);


        if(!Input.GetMouseButton(0)) {                
            yMove = yRotation * transform.up.normalized;
            xMove = Vector3.right*xSin + -Vector3.forward*xCos;
        }

        Vector3 newPos = xMove + yMove;
        Vector3 freeLookPos = target.position + newPos.normalized*distanceToTarget;
        transform.position = Vector3.SmoothDamp(transform.position,freeLookPos, ref smoothVelocity,freeLookSmooth);

    }

    void FollowCamera( Vector3 wanted) {
        Vector3 wantedPosition = wanted;
        distanceToTarget = Vector3.Distance(transform.position,target.position);
        distance = origDist;
        transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref smoothVelocity, smoothSpeed);

        xRotation = -target.eulerAngles.y*Mathf.Deg2Rad;
        yRotation = 0.2f;
    }

    void CharacterFollow(Vector3 wanted) {

        //X-Axis Rotation
        xRotation = xRotation + (Input.GetAxis("Mouse X") * xRotSpeed);
        xSin = Mathf.Sin(xRotation);
        xCos = Mathf.Cos(xRotation);

        //Y-Axis Rotation
        float maxYRads = maxYRot3rd*Mathf.Deg2Rad;
        float minYRads = minYRot3rd*Mathf.Deg2Rad;
        yRotation = yRotation + Input.GetAxis("Mouse Y") * yRotSpeed;
        yRotation = Mathf.Clamp(yRotation, minYRads, maxYRads);

        yMove = yRotation * transform.up.normalized;
        xMove = -Vector3.right*xSin + -Vector3.forward*xCos;

        Vector3 newPos = xMove + yMove;
        Vector3 freeLookPos = target.position + newPos.normalized*distance*characterDistanceFactor;
        //transform.position = Vector3.SmoothDamp(transform.position,freeLookPos, ref smoothVelocity,freeLookSmooth);
    }


    public void CharacterInDialogue(bool inDialogue) {
        still = inDialogue;
    }


    
}
