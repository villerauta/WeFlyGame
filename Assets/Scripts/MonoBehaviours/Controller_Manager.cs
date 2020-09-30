using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly {
    public class Controller_Manager : MonoBehaviour
    {
        private Basic_Follow_Camera mainCamera;
        private WeFly.Character_Input character;
        private WeFly.Airplane_Controller airplane;

        private enum Controlled {
            None,
            Alfonso,
            Plane
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            mainCamera = FindObjectOfType<Basic_Follow_Camera>();
            airplane = FindObjectOfType<WeFly.Airplane_Controller>();
            character = FindObjectOfType<WeFly.Character_Input>();
        }
        public void CharacterBoardingPlane() {
            //Character Controller needs to be disabled to move the player through transform.position
            airplane.boarded = true;
            character.controller.enabled = false;
            character.transform.SetParent(airplane.transform);
            ChangeControl(Controlled.Plane);
            mainCamera.target = airplane.transform;
        } 
        public void CharacterGettingOffPlane() {
            //Character controller enabled AFTER transform.position
            airplane.boarded = false;
            character.transform.SetParent(null);
            character.transform.position = airplane.transform.position;
            character.controller.enabled = true;
            ChangeControl(Controlled.Alfonso);
            mainCamera.target = character.cameraFollowPoint.transform;
        }

        public void CharacterInDialogue(bool inDialogue) {
            character.allowMovement = !inDialogue;
            mainCamera.CharacterInDialogue(inDialogue);
        }

        void ChangeControl(Controlled controlled) {
            
            switch (controlled) {
                case Controlled.None:
                    Debug.Log("None in control");
                    character.allowMovement = false;
                    airplane.input.isActive = false;
                    break;
                case Controlled.Alfonso:
                    airplane.input.isActive = false;
                    character.allowMovement = true;
                    break;
                case Controlled.Plane:
                    character.allowMovement = false;
                    airplane.input.isActive = true;
                    break;
            }
        }

        
    }

}
