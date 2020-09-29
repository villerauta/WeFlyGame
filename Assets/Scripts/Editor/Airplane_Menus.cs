using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace WeFly {
    public class Airplane_Menus
    {
        [MenuItem("Airplane Tools/Create New Airplane")]
        public static void CreateNewAirplane()
        {
            GameObject curSelected = Selection.activeGameObject;
            if (curSelected) {
                
                Airplane_Controller curController = curSelected.AddComponent<Airplane_Controller>();
                GameObject curCOG = new GameObject("COG");
                curCOG.transform.SetParent(curSelected.transform);

                curController.centerOfGravity = curCOG.transform;
            }
        }
    }
}