using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_Engine : MonoBehaviour
{

    #region Variables
    [Header("Engine Properties")]
    public float maxForce = 200f;
    public float maxRPM = 2550f;
    public AnimationCurve powerCurve = AnimationCurve.Linear(0f,0f,1f,1f);

    [Header("Propellers")]
    public Airplane_Propeller propeller;
    #endregion

    #region 
    #endregion


    #region Custom Methods
    public Vector3 CalculateForce(float throttle) {
        
        float finalThrottle = Mathf.Clamp01(throttle);
        finalThrottle = powerCurve.Evaluate(finalThrottle);
        float currentRPM = finalThrottle * maxRPM;

        if (propeller) {
            propeller.HandlePropeller(currentRPM);
        }

        float finalPower = finalThrottle * maxForce;
        Vector3 finalForce = transform.forward * finalPower;
        return finalForce;
    }


    #endregion
}
