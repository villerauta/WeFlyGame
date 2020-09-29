using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly {
    public class Controller_Manager : MonoBehaviour
    {
        private Basic_Follow_Camera mainCamera;
        public WeFly.Character_Input character;
        public CharacterController characterController;
        private WeFly.Airplane_Controller airplane;
        public Transform airplaneTransform;
        private bool characterOnBoard;

        private Controlled currentControlled = Controlled.Alfonso;


        private enum Controlled {
            None,
            Alfonso,
            Plane
        }

        IEnumerator WaitForPlaneInteraction() {
            
            Debug.Log("Coroutine started");
            yield return new WaitForSeconds(2f);
            Debug.Log("Coroutine ended");
        }

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
            mainCamera = FindObjectOfType<Basic_Follow_Camera>();
            airplane = FindObjectOfType<WeFly.Airplane_Controller>();
            airplane.HandlePlaneBoarded(false);
            character = FindObjectOfType<WeFly.Character_Input>();
        }

        public void InteractWithPlane() {
            switch (currentControlled) {
                case Controlled.None:
                    Debug.Log("Error: None in control");
                    break;
                case Controlled.Alfonso:
                    //Alfonso in control, so board the plane
                    CharacterBoardingPlane();
                    StartCoroutine(WaitForPlaneInteraction());
                    break;
                case Controlled.Plane:
                    //Plane in control, so unboard Alfonso
                    CharacterGettingOffPlane();
                    StartCoroutine(WaitForPlaneInteraction());
                    break;
            }
        }
        public void CharacterBoardingPlane() {
            //Character Controller needs to be disabled to move the player through transform.position
            characterController.enabled = false;
            ChangeControl(Controlled.Plane);
            airplane.HandlePlaneBoarded(true);
            mainCamera.target = airplaneTransform;
            //characterOnBoard = true;
        } 
        public void CharacterGettingOffPlane() {
            //Character controller enabled AFTER transform.position
            characterController.transform.position = airplane.transform.position;
            characterController.enabled = true;
            ChangeControl(Controlled.Alfonso);
            airplane.HandlePlaneBoarded(false);
            mainCamera.target = character.transform;
            characterOnBoard = false;
        }

        public void CharacterInDialogue(bool inDialogue) {
            character.allowMovement = !inDialogue;
            mainCamera.CharacterInDialogue(inDialogue);
        }

        void ChangeControl(Controlled controlled) {
            
            switch (controlled) {
                case Controlled.None:
                    currentControlled = Controlled.None;
                    Debug.Log("None in control");
                    character.isActive = false;
                    airplane.input.isActive = false;
                    break;
                case Controlled.Alfonso:
                    currentControlled = Controlled.Alfonso;
                    Debug.Log("Alfonso in control");
                    airplane.input.isActive = false;
                    character.isActive = true;
                    break;
                case Controlled.Plane:
                    currentControlled = Controlled.Plane;
                    Debug.Log("Plane in control");
                    character.isActive = false;
                    airplane.input.isActive = true;
                    break;
            }
        }

        
    }

}
