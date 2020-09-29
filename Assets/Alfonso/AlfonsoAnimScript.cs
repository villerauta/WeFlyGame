using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlfonsoAnimScript : MonoBehaviour
{
    private Animator animator;
    public float gravity = 8f;
    private Vector3 gravityForce = Vector3.zero;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gravityForce = new Vector3(0,-gravity*Time.deltaTime,0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        // The speed on the x-z plane ignoring any speed
        float horizontalSpeed = horizontalVelocity.magnitude;
        if (horizontalSpeed > 0.1f) {
            animator.SetBool("isRunning",true);
        } else {
            animator.SetBool("isRunning",false);
        }
        
        
        controller.Move(gravityForce);
    }
}
