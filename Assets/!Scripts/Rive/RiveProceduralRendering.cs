using Rive;
using UnityEngine;
using UnityEngine.Rendering;
using Color = Rive.Color;
using Renderer = Rive.Renderer;
using RenderQueue = Rive.RenderQueue;


namespace _Scripts.Rive
{

    public class RiveProceduralRendering : MonoBehaviour
    {
        RenderTexture _mRenderTexture;
        RenderQueue _mRenderQueue;
        Renderer _mRiveRenderer;
        CommandBuffer _mCommandBuffer;

        Camera _mCamera;

        Path _mPath;
        Paint _mPaint;

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

        void Start()
        {
            _mRenderTexture = new RenderTexture(TextureHelper.Descriptor(256, 256));
            _mRenderTexture.Create();

            UnityEngine.Renderer cubeRenderer = GetComponent<UnityEngine.Renderer>();
            Material mat = cubeRenderer.material;
            mat.mainTexture = _mRenderTexture;

            if (!FlipY())
            {
                // Flip the render texture vertically for OpenGL
                mat.mainTextureScale = new Vector2(1, -1);
                mat.mainTextureOffset = new Vector2(0, 1);
            }

            _mRenderQueue = new RenderQueue(_mRenderTexture);
            _mRiveRenderer = _mRenderQueue.Renderer();

            _mPath = new Path();
            _mPaint = new Paint();
            _mPaint.Color = new Color(0xFFFFFFFF);
            _mPaint.Style = PaintingStyle.stroke;
            _mPaint.Join = StrokeJoin.round;
            _mPaint.Thickness = 20.0f;

            _mRiveRenderer = _mRenderQueue.Renderer();
            _mRiveRenderer.Draw(_mPath, _mPaint);

            _mCommandBuffer = _mRiveRenderer.ToCommandBuffer();
            _mCommandBuffer.SetRenderTarget(_mRenderTexture);
            _mRiveRenderer.AddToCommandBuffer(_mCommandBuffer, true);
            _mCamera = Camera.main;
            if (_mCamera != null)
            {
                Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, _mCommandBuffer);
            }
        }

        const int TriangleStart = 125;
        const int TriangleSize = 50;
        const float GrowSize = 20.0f;

        void Update()
        {
            if (_mPath == null)
            {
                return;
            }
            _mPath.Reset();

            float expand = (Mathf.Sin(Time.fixedTime * Mathf.PI * 2) + 1.0f) * 20.0f + 1.0f;
            _mPath.MoveTo(TriangleStart, TriangleStart - TriangleSize - expand);
            _mPath.LineTo(TriangleStart + TriangleSize + expand, TriangleStart + TriangleSize + expand);
            _mPath.LineTo(TriangleStart - TriangleSize - expand, TriangleStart + TriangleSize + expand);
            _mPath.Close();

            _mPaint.Thickness = (Mathf.Sin(Time.fixedTime * Mathf.PI * 2) + 1.0f) * 20.0f + 1.0f;

            _mRiveRenderer = _mRenderQueue.Renderer();
            _mRiveRenderer.Draw(_mPath, _mPaint);

            _mCommandBuffer.Clear();
            _mCommandBuffer.SetRenderTarget(_mRenderTexture);
            _mRiveRenderer.AddToCommandBuffer(_mCommandBuffer, true);
        }

        void OnDisable()
        {
            if (_mCamera != null && _mCommandBuffer != null)
            {
                _mCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, _mCommandBuffer);
            }
        }

        void OnDestroy()
        {
            // Release the RenderTexture when it's no longer needed
            if (_mRenderTexture != null)
            {
                _mRenderTexture.Release();
            }
        }
    }

}
