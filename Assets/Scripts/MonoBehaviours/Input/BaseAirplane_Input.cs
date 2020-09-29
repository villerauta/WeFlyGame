using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly
{
    public class BaseAirplane_Input : MonoBehaviour
    {
        public bool isActive = false;
        #region variables
        protected float pitch = 0f;
        protected float roll = 0f;
        protected float yaw = 0f;
        protected float throttle = 0f;
        protected float flaps = 1;
        protected float brake = 0f;
        //protected bool freeLook = false;
        #endregion

        public float maxPitchValue = 1f;
        public float maxRollValue = 1f;

        public float throttleUpSpeed = 0.7f;
        public float throttleDownSpeed = 0.01f;

        public float throttleDownSpeedFactor = 8f;
        private float lingerThrottle;
        private float planeSpeed;
        public float StickyThrottle {
            get {return lingerThrottle;}
        }

        #region getters
        public float Pitch
        {
            get{return pitch;}
        }
        public float Roll
        {
            get{return roll;}
        }
        public float Yaw
                {
                    get{return yaw;}
                }
        public float Throttle
                {
                    get{return throttle;}
                }
        public float Flaps
                {
                    get{return flaps;}
                }
        public float Brake
                {
                    get{return brake;}
                }
        //public bool FreeLook {get{return freeLook;}}
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(isActive) {
                HandleInput();
            }
        }

        public void SetPlaneSpeed(float speed) {
            planeSpeed = speed;
        }

        void HandleInput() {
            if(Input.GetMouseButton(0)) {
                    pitch = pitch + Input.GetAxis("Mouse Y");
                    pitch = Mathf.Clamp(pitch, -maxPitchValue, maxPitchValue);

                    roll = roll + Input.GetAxis("Mouse X");
                    roll = Mathf.Clamp(roll, -maxRollValue, maxRollValue);
            } else {
                pitch = 0f;
                roll = 0f;
            }
            yaw = Input.GetAxis("Horizontal");
            ThrottleControl();

            //AirBrake
            flaps = Input.GetKey(KeyCode.Space)? 1f: 0f;
            brake = Input.GetKey(KeyCode.Space)? 1f: 0f;
            
        }

        void ThrottleControl() {
            if(Input.GetKey(KeyCode.W)) {
                lingerThrottle = lingerThrottle + (throttleUpSpeed * Time.deltaTime);
            } else {
                lingerThrottle = lingerThrottle - ((1f+(planeSpeed*throttleDownSpeedFactor)) * throttleDownSpeed * Time.deltaTime);    
            }
            lingerThrottle = Mathf.Clamp01(lingerThrottle);
            
            
        }
    }
}
