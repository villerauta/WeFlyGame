using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_Propeller : MonoBehaviour
{
    void Start() {
        if(mainProp && blurProp) {  
            HandleSwapping(0f);
        }
    }

    [Header("Properties")]
    public float minQuadRPM = 300f;
    public float minTextureSwap = 600f;
    public GameObject mainProp;
    public GameObject blurProp;
    public Material blurredProprMAT;
    public Texture2D blurLevel1;
    public Texture2D blurLevel2;

    public void HandlePropeller(float currentRPM) {
        float dps = 10f + ((currentRPM * 360) / 60f) * Time.deltaTime;
        transform.Rotate(Vector3.forward,dps);

        if(mainProp && blurProp) {  
            HandleSwapping(currentRPM);
        }
    }

    void HandleSwapping (float currentRPM) {
        if (currentRPM > minQuadRPM) {
            blurProp.gameObject.SetActive(true);
            mainProp.gameObject.SetActive(false);

            if(blurredProprMAT &&  blurLevel1 && blurLevel2){
                if (currentRPM > minTextureSwap) {
                    blurredProprMAT.SetTexture("MainTex",blurLevel2);
                }
            } else {
                blurredProprMAT.SetTexture("MainTex",blurLevel1);
            }

        } else {
            blurProp.gameObject.SetActive(false);
            mainProp.gameObject.SetActive(true);    
        }
    }
}
