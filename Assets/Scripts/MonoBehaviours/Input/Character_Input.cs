using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly {
    public class Character_Input : MonoBehaviour
    {
        public Animator animator;
        public Transform cam;
        public CharacterController controller;
        public WeFly.Controller_Manager controller_Manager;

        //Movement variables
        public float speed = 5f;
        protected Vector3 direction;
        private Vector3 playerVelocity;
        public float turnSmoothTime = 0.1f;
        public float turnSmoothVelocity;

        //Gravitational force
        public float gravity = 5f;
    
        //Character State
        public bool isActive = true;
        public bool allowMovement = true;
        private bool _inDialogue = false;
        public bool onAirplane = false;

        //Interaction variables
        public TextMesh text;
        public float senseDistance = 3f;
        private Ray ray;
        private RaycastHit hit;
        protected Vector3 interactionDirection;
        private WaitForSeconds interactionHold;

        public Vector3 Direction
            {
                get{return direction;}
            }


        private void Start() {
            interactionHold = new WaitForSeconds(3f);
            controller.detectCollisions = false;
        }
        void Update()
        {
            if(isActive){
                HanldeMovement();
                HandleInteraction();
            }
            
        }
        void HanldeMovement() {
            if(allowMovement) {
                //Calculate movement direction
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical")).normalized;

                if (direction.magnitude >= 0.1f) {
                    //Movement
                    animator.SetBool("isRunning",true);

                    //Model ange
                    float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f,angle,0f);

                    //Move player
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                } else {
                    // Not moving -> isRunning false
                    animator.SetBool("isRunning",false);
                }
                //Gravity pulls player down
                playerVelocity.y -= gravity;
                controller.Move(playerVelocity*Time.deltaTime);
            }
        }

        void HandleInteraction() {
            //
            // Handle character interaction and sensing interactables
            //
            interactionDirection = cam.transform.forward;
            interactionDirection.y = 0f;

            //TODO Make char model with origin in the geometric centre
            Vector3 torso = transform.position;
            torso.y += 0.5f;

            ray = new Ray(torso,interactionDirection);
            Debug.DrawRay(torso,interactionDirection,Color.blue);

            if(Physics.Raycast(ray,out hit,senseDistance)){
                
                //Check what Raycast hit
                //TODO: Change to look for "Interactables" only
                if(hit.collider.tag == "plane") {
                    text.text = "Press E to board the plane";
                     if(Input.GetKeyDown(KeyCode.E)) {
                        controller_Manager.InteractWithPlane();
                    } 
                 } else if (hit.collider.tag == "NPC") {
                    if(Input.GetKeyDown(KeyCode.E) & !_inDialogue) {
                        _inDialogue = true;
                        //hit.collider.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
                    }
                } 

            } else {
                text.text = "";
            }
        }

    }
}
