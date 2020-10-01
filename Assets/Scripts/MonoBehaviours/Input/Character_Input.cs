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
        public WeFly.Airplane_Controller airplane_Controller;
        public CameraFollowPoint cameraFollowPoint;

        public SaveData playerSaveData;

        //Movement variables
        public float speed = 5f;
        protected Vector3 direction;
        private Vector3 playerVelocity;
        public float turnSmoothTime = 0.1f;
        public float turnSmoothVelocity;

        //Gravitational force
        public float gravity = -5f;
    
        //Character State
        public bool isActive = true;
        public bool allowMovement = true;
        public bool _allowInput = true;
        public bool allowInteraction = true;
        private bool _inDialogue = false;
        public bool _inAirplane = false;
        public bool _isJumping = false;

        private Vector3 jumpTarget = Vector3.zero;
        private Vector3 jumpStart = Vector3.zero;
        private float t = 1f;

        //Interaction variables
        public TextMesh text;
        public float senseDistance = 3f;
        private Ray ray;
        private RaycastHit hit;
        protected Vector3 interactionDirection;
        private WaitForSeconds interactionHold;
        private WaitUntil waitForJump;
        private WaitForSeconds jumpHold;

        public const string startingPositionKey = "starting position";

        private Interactable aimedInteractable;

        public Vector3 Direction
            {
                get{return direction;}
            }

        IEnumerator WaitForInteraction() {
            
            allowInteraction = false;
            yield return interactionHold;
            allowInteraction = true;
        }

        IEnumerator PlaneInteraction() {
            jumpStart = transform.position;
            t = 0;
            controller.enabled = false;

            //isJumping to override normal movement
            _isJumping = true;

            yield return interactionHold;
            _isJumping = false;

            Debug.Log("Jump stop");
            _inAirplane = true;
            controller.enabled = true;

        }

        private void Start() {
            interactionHold = new WaitForSeconds(0.5f);

            controller.detectCollisions = false;

            string startPosName = "";
            playerSaveData.Load(startingPositionKey, ref startPosName);
            Transform startingPosition = StartingPosition.FindStartingPosition(startPosName);

            transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;

        }
        void Update()
        {
            if(_isJumping) {
                HandleJump();
                Debug.Log("Jumping");
            } 
            else {
            
                if (allowMovement){
                    HanldeMovement();
                }
                if (allowInteraction) {
                    HandleInteraction(); 
                }
                if (controller.isGrounded && playerVelocity.y < 0) {
                    playerVelocity.y = 0;
                }
            }    
        }

        void HandleJump(){
            t += Time.deltaTime/0.5f;
            transform.position = Vector3.Lerp(jumpStart, jumpTarget, t);
             
            /* playerVelocity.y += gravity*4 * Time.deltaTime;
            controller.Move(playerVelocity*Time.deltaTime); */
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
                playerVelocity.y += gravity*4 * Time.deltaTime;
                controller.Move(playerVelocity*Time.deltaTime);
            }
        }


        void HandleInteraction() {
            // Handle character interaction and sensing interactables

            if (_inAirplane) {
                if (airplane_Controller.characteristics.normalizedMPH < 0.03f) {
                    text.text = "Exit";
                    if(Input.GetKeyDown(KeyCode.E)) {
                        text.text = "";
                        StartCoroutine(WaitForInteraction());
                        controller.enabled = true;
                        controller_Manager.CharacterGettingOffPlane();
                        _inAirplane = false;
                    }
                } 
                else {
                    text.text = "";
                }
            } 
            else {
                interactionDirection = cam.transform.forward;
                interactionDirection.y = 0f;

                //TODO Make char model with origin in the geometric centre
                Vector3 torso = transform.position;
                torso.y += 0.5f;

                ray = new Ray(torso,interactionDirection);
                Debug.DrawRay(torso,interactionDirection,Color.red);

                if(Physics.Raycast(ray,out hit,senseDistance)){
                    
                    //Check what Raycast hit
                    //TODO: Change to look for "Interactables" only
                    if(hit.collider.GetComponent<Interactable>() != null) {
                        aimedInteractable = hit.collider.GetComponent<Interactable>();
                        text.text = aimedInteractable.interactionText;
                        if (Input.GetKeyDown(KeyCode.E)) {
                            text.text = "";
                            StartCoroutine(WaitForInteraction());
                            aimedInteractable.Interact();
                        }
                    }
                    if(hit.collider.tag == "plane") {
                        text.text = "Enter";
                        if(Input.GetKeyDown(KeyCode.E)) {
                            jumpTarget = airplane_Controller.sittingPos.transform.position;
                            text.text = "";
                            StartCoroutine(PlaneInteraction());
                            controller_Manager.CharacterBoardingPlane();
                            
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
}
