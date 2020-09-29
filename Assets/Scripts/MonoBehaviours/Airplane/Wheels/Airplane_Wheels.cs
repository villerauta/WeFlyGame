using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly {
    [RequireComponent(typeof(WheelCollider))]
    public class Airplane_Wheels : MonoBehaviour
    {

        [Header("WHeel Properties")]
        public Transform WheelGraphic;
        public bool isBraking = false;
        public float brakePower = 500f;
        public bool isSteering = false;
        public float steeringAngle = 20f;

        private WheelCollider WheelCol;
        private Vector3 worldPos;
        private Quaternion worldRot;

        private float finalBrakeForce;

        void Start() {
            WheelCol = GetComponent<WheelCollider>();
        }

        public void InitWheel() {
            if (WheelCol) {
                WheelCol.motorTorque = 0.0000000001f;
            }
        }

        public void HandleWheel(BaseAirplane_Input input) {
            if (WheelCol) {
                WheelCol.GetWorldPose(out worldPos, out worldRot);
                if (WheelGraphic) {
                    WheelGraphic.rotation = worldRot;
                    WheelGraphic.position = worldPos;
                }
                
                if (isBraking )
                {    
                    if (input.Brake > 0.1f) {
                        finalBrakeForce = Mathf.Lerp(finalBrakeForce, input.Brake * brakePower, Time.deltaTime);
                        WheelCol.brakeTorque = finalBrakeForce;
                    } else {
                        finalBrakeForce = 0f;
                        WheelCol.brakeTorque = 0f;
                        WheelCol.motorTorque = 0.000000001f;
                    }
                }

                if(isSteering) {
                    WheelCol.steerAngle = -input.Yaw * steeringAngle;
                }
            }
        }
        
    }
}