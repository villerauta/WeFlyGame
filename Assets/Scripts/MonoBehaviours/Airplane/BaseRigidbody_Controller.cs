using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFly
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class BaseRigidbody_Controller : MonoBehaviour
    {
        protected Rigidbody rb;
        protected AudioSource aSource;


        protected virtual void HandlePhysics() {}


        // Start is called before the first frame update
        public virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            aSource = GetComponent<AudioSource>();
            if (aSource) {
                aSource.playOnAwake = false;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (rb) {
                HandlePhysics();
            }
        }
    }
 }   

