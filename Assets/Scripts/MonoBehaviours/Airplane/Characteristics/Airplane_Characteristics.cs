using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly {
    public class Airplane_Characteristics : MonoBehaviour
    {
        private Rigidbody rb;
        private float startDrag;
        private float startAngularDrag;

        const float mpsToMph = 2.23694f;


        [Header("Characteristics Properties")]
        public float forwardSpeed;
        public float realForwardSpeed;
        public float maxForwardSpeed;
        public float mph;
        public float maxMPH = 200;
        public float rbVeloLerpSpeed = 0.2f;
        public float rbRotaLerpSpeed = 0.8f;

        public float normalizedMPH;

        [Header("Lift Properties")]
        public float maxLiftPower = 4000f;
        public AnimationCurve liftCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);

        [Header("Drag Properties")]
        public float dragFactor = 1f;
        public float angularFactor = 1f;
        public float angularOffset = 0.2f;
        public float flapDragFacotr = 0.08f;

        [Header("Control Properties")]
        public float pitchSpeed = 1200f;
        public float rollSpeed = 1200f;
        public float yawSpeed = 650f;
        public float bankSpeed = 500f;

        private BaseAirplane_Input input;


        private float maxMPS;

        private float angleOfAttack;

        private float pitchAngle;
        private float rollAngle;

        public void InitCharacteristics(Rigidbody curRB, BaseAirplane_Input curInput){
            input = curInput;
            maxMPS = maxMPH / mpsToMph;
            rb = curRB;
            startDrag = rb.drag;
            startAngularDrag = rb.angularDrag;
        }
        
        public void UpdateCharacteristics() {

            if (rb){    
                CalculateForwardSpeed();
                CalculateLift();
                CalculateDrag();
                HandlePitch();
                HandleRoll();
                HandleYaw();
                HandleBanking();

                HandleRigidbodyTransform();
            }
        }



        void CalculateForwardSpeed() {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            
            forwardSpeed = Mathf.Max(0f,localVelocity.z);
            realForwardSpeed = Mathf.Max(0f,localVelocity.z);
            
            forwardSpeed = Mathf.Clamp(forwardSpeed,0f,maxMPS);
            mph = mpsToMph * forwardSpeed;

            mph = Mathf.Clamp(mph,0f,maxMPH);
            normalizedMPH = Mathf.InverseLerp(0f, maxMPH, mph);
            
        }

        void CalculateLift() {
            
            //Angle of Attack
            angleOfAttack = Vector3.Dot(rb.velocity.normalized, transform.forward);
            angleOfAttack *= angleOfAttack;

            Vector3 liftDir = transform.up;
            float liftPower = liftCurve.Evaluate(normalizedMPH) * maxLiftPower;
            Vector3 liftForce = liftDir * liftPower * angleOfAttack;
            rb.AddForce(liftForce);
            
        }

        void CalculateDrag() {
            float speedDrag = realForwardSpeed * 0.0001f * dragFactor;

            //TODO: timeDelta to flapDrag
            float flapDrag = input.Flaps * flapDragFacotr;
            float finalDrag = startDrag + speedDrag + flapDrag;
            rb.drag = finalDrag;

            rb.angularDrag = angularOffset + (startAngularDrag * realForwardSpeed * (angularFactor));

        }

        void HandleRigidbodyTransform() {
            if (rb.velocity.magnitude > 1) {

                //Velocity Lerp
                Vector3 updatedVelocity = Vector3.Lerp(rb.velocity, transform.forward * forwardSpeed, forwardSpeed * angleOfAttack * Time.deltaTime * rbVeloLerpSpeed);
                rb.velocity = updatedVelocity;

                //Rotation Lerp
                Quaternion updatedRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, transform.up), Time.deltaTime * rbRotaLerpSpeed);
                rb.MoveRotation(updatedRotation);
            }

        }

        void HandlePitch() {
            Vector3 flatForward = transform.forward;
            flatForward.y = 0;
            flatForward = flatForward.normalized;
            pitchAngle = Vector3.Angle(transform.forward, flatForward);
            
            Vector3 pitchTorque = input.Pitch * pitchSpeed * transform.right;
            rb.AddTorque(pitchTorque);
        }

        void HandleRoll() {
            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            flatRight = flatRight.normalized;
            rollAngle = Vector3.SignedAngle(transform.right, flatRight, transform.forward);

            Vector3 rollTorque = -input.Roll * rollSpeed * transform.forward;
            rb.AddTorque(rollTorque);
        }

        void HandleYaw() {
            Vector3 yawTorque = input.Yaw * yawSpeed * transform.up;
            rb.AddTorque(yawTorque);
        }

        void HandleBanking() {
            float bankSide = Mathf.InverseLerp(-90f,90f,rollAngle);
            float bankAmount = Mathf.Lerp(-1f,1f,bankSide);
            Vector3 bankTorque = bankAmount * bankSpeed * transform.up;
            rb.AddTorque(bankTorque);
        }
    }
}