using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeFly{
    [RequireComponent(typeof(Airplane_Characteristics))]
    public class Airplane_Controller : BaseRigidbody_Controller
    {
        #region Variables
        [Header("Game Logic Components")]
        public Controller_Manager controller_Manager;
        public WeFly.Character_Input character_Input;
        public Transform sittingPos;
        public Transform exitPos;
        public GameObject CollisionGroup;

        [Header("Base Airplane Properties")]
        public BaseAirplane_Input input;
        public Airplane_Characteristics characteristics;
        public Transform centerOfGravity;
        public Transform interactionCollider;

        [Tooltip("Kg")]
        public float airplaneweight = 800f;
        #endregion

        [Header("Engines")]
        public List<Airplane_Engine> engines = new List<Airplane_Engine>();

        [Header("Wheels")]
        public List<Airplane_Wheels> wheels = new List<Airplane_Wheels>();

        public bool boarded;

        public override void Start()
        {
            base.Start();
            if (rb) {
                rb.mass = airplaneweight;
                if (centerOfGravity) {
                    rb.centerOfMass = centerOfGravity.localPosition;
                }

                characteristics = GetComponent<Airplane_Characteristics>();
                if (characteristics) {
                    characteristics.InitCharacteristics(rb, input);
                }
            }

            if (wheels != null) {
                if (wheels.Count > 0) {
                    foreach(Airplane_Wheels wheel in wheels) {
                        wheel.InitWheel();
                    }
                }
            }

        }

        protected override void HandlePhysics()
        {
            if (input) {
                HandleEngines();
                HandleCharacteristics();
                HandleWheel();
                input.SetPlaneSpeed(characteristics.normalizedMPH);
            }
        }

        void HandleEngines() {

            if (engines != null) {
                if(engines.Count > 0) {
                    foreach(Airplane_Engine engine in engines) {
                        Vector3 forwardForce = engine.CalculateForce(input.StickyThrottle);
                        rb.AddForce(forwardForce);
                    }
                }
            }
        }

        void HandleCharacteristics() {
            if (characteristics) {
                characteristics.UpdateCharacteristics();
            }
        }

        void HandleWheel() {
            if (wheels.Count > 0){
                foreach(Airplane_Wheels wheel in wheels) {
                    wheel.HandleWheel(input, !boarded);
                }
            }
        }
    }
}
