using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ArcadeVehiclePhysics.SampleLevel.OutlineShader
{
    public class OutlineEffect : MonoBehaviour
    {
        Camera _camera;
        public Material material;

        public List<ShaderVariable> shaderVariables = new List<ShaderVariable>();

        void Awake()
        {
            if (_camera != null)
                return;
            _camera = GetComponent<Camera>();
            _camera.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        // Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            foreach (var shaderVariable in shaderVariables.Where(shaderVariable => shaderVariable.varName != ""))
            {
                material.SetFloat(shaderVariable.varName, shaderVariable.varValue);
            }

            Graphics.Blit(source, null as RenderTexture, material);
        }

        [System.Serializable]
        public class ShaderVariable
        {
            public string varName;
            public float varValue;
        }
    }
}
