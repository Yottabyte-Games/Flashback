using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
    public class FMODRuntimeManagerOnGUIHelper : MonoBehaviour
    {
        public RuntimeManager TargetRuntimeManager = null;

        void OnGUI()
        {
            if (TargetRuntimeManager)
            {
                TargetRuntimeManager.ExecuteOnGUI();
            }
        }
    }
}
