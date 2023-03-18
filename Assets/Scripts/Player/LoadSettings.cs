using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class LoadSettings : MonoBehaviour
    {
        public PlayerSettings playerSettings;
        public UniversalAdditionalCameraData playerCamera;

        bool postProcessing = true;

        private void Start()
        {
            postProcessing = playerSettings.postProcessing;
            playerCamera.renderPostProcessing = postProcessing;
        }
    }
}