using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityAmount = 8f;
    private CharacterController characterController;
    private Vector3 gravity;
    void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        gravity = new Vector3(0,-gravityAmount*Time.deltaTime,0);
    }

    void Update()
    {
        characterController.Move(gravity);
    }
}
