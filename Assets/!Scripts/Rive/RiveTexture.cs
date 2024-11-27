using Rive;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts.Rive
{
    public class RiveTexture : MonoBehaviour
    {
        public Asset asset;
        public Fit fit = Fit.Contain;
        public int size = 512;

        RenderTexture _mRenderTexture;
        global::Rive.RenderQueue _mRenderQueue;
        global::Rive.Renderer _mRiveRenderer;
        CommandBuffer _mCommandBuffer;

        File _mFile;
        Artboard _mArtboard;
        StateMachine _mStateMachine;
        public StateMachine stateMachine => _mStateMachine;

        static bool FlipY()
        {
            switch (SystemInfo.graphicsDeviceType)
            {
                case GraphicsDeviceType.Metal:
                case GraphicsDeviceType.Direct3D11:
                    return true;
                default:
                    return false;
            }
        }

        void Awake()
        {
            _mRenderTexture = new RenderTexture(TextureHelper.Descriptor(size, size));
            _mRenderTexture.Create();

            UnityEngine.Renderer renderer = GetComponent<UnityEngine.Renderer>();
            Material material = renderer.material;
            material.mainTexture = _mRenderTexture;

            if (!FlipY())
            {
                // Flip the render texture vertically for OpenGL
                material.mainTextureScale = new Vector2(1, -1);
                material.mainTextureOffset = new Vector2(0, 1);
            }

            _mRenderQueue = new global::Rive.RenderQueue(_mRenderTexture);
            _mRiveRenderer = _mRenderQueue.Renderer();
            if (asset != null)
            {
                _mFile = File.Load(asset);
                _mArtboard = _mFile.Artboard(0);
                _mStateMachine = _mArtboard?.StateMachine();
            }

            if (_mArtboard != null && _mRenderTexture != null)
            {
                _mRiveRenderer.Align(fit, Alignment.Center, _mArtboard);
                _mRiveRenderer.Draw(_mArtboard);

                _mCommandBuffer = new CommandBuffer();
                _mCommandBuffer.SetRenderTarget(_mRenderTexture);
                _mCommandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                _mRiveRenderer.AddToCommandBuffer(_mCommandBuffer);
            }
        }

        void Update()
        {
            _mRiveRenderer.Submit();
            GL.InvalidateState();
        
            if (_mStateMachine != null)
            {
                _mStateMachine.Advance(Time.deltaTime);
            }
        }
    }
}
