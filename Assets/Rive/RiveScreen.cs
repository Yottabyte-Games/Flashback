using System;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace Rive
{
    internal class CameraTextureHelper
    {
        Camera _mCamera;
        RenderTexture _mRenderTexture;
        int _mPixelWidth = -1;
        int _mPixelHeight = -1;
        RenderQueue _mRenderQueue;

        // Queue to keep things on the main thread only.
        static ConcurrentQueue<Action> _mainThreadActions = new ConcurrentQueue<Action>();

        public RenderTexture renderTexture
        {
            get { return _mRenderTexture; }
        }

        public Camera camera
        {
            get { return _mCamera; }
        }

        internal CameraTextureHelper(Camera camera, RenderQueue queue)
        {
            _mCamera = camera;
            _mRenderQueue = queue;
            UpdateTextureHelper();
        }

        ~CameraTextureHelper()
        {
            // Since the GC calls the destructor and doesn't run on the main thread,
            // we need to ensure the cleanup() call happens on the main thread.
            _mainThreadActions.Enqueue(() => Cleanup());
        }

        void Cleanup()
        {
            if (_mRenderTexture != null)
            {
                _mRenderTexture.Release();
            }
        }

        void Update()
        {
            // Process main thread actions
            while (_mainThreadActions.TryDequeue(out var action))
            {
                action();
            }
        }

        public bool UpdateTextureHelper()
        {

            if (_mPixelWidth == _mCamera.pixelWidth && _mPixelHeight == _mCamera.pixelHeight)
            {
                return false;
            }
        
            Cleanup();

            _mPixelWidth = _mCamera.pixelWidth;
            _mPixelHeight = _mCamera.pixelHeight;
            var textureDescriptor = TextureHelper.Descriptor(_mPixelWidth, _mPixelHeight);
            _mRenderTexture = new RenderTexture(textureDescriptor);
            _mRenderTexture.Create();
            _mRenderQueue.UpdateTexture(_mRenderTexture);

            return true;
        }

    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
// Draw a Rive artboard to the screen. Must be bound to a camera.
    public class RiveScreen : MonoBehaviour
    {
        public Asset asset;
        public CameraEvent cameraEvent = CameraEvent.AfterEverything;
        public Fit fit = Fit.contain;
        public Alignment Alignment = Alignment.Center;
        public event RiveEventDelegate OnRiveEvent;
        public delegate void RiveEventDelegate(ReportedEvent reportedEvent);

        RenderQueue _mRenderQueue;
        Renderer _mRiveRenderer;
        CommandBuffer _mCommandBuffer;

        File _mFile;
        Artboard _mArtboard;
        StateMachine _mStateMachine;
        CameraTextureHelper _mHelper;

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

        void OnGUI()
        {
            if (_mHelper is not null && Event.current.type.Equals(EventType.Repaint))
            {
                var texture = _mHelper.renderTexture;

                var width = _mHelper.camera.scaledPixelWidth;
                var height = _mHelper.camera.scaledPixelHeight;

                GUI.DrawTexture(
                    FlipY() ? new Rect(0, height, width, -height) : new Rect(0, 0, width, height),
                    texture,
                    ScaleMode.StretchToFill,
                    true
                );

            }

        }

        void Awake()
        {
            if (asset != null)
            {
                _mFile = File.Load(asset);
                _mArtboard = _mFile.Artboard(0);
                _mStateMachine = _mArtboard?.StateMachine();
            }

            Camera camera = gameObject.GetComponent<Camera>();
            Assert.IsNotNull(camera, "RiveScreen must be attached to a camera.");

            // Make a RenderQueue that doesn't have a backing texture and does not
            // clear the target (we'll be drawing on top of it).
            _mRenderQueue = new RenderQueue(null, false);
            _mRiveRenderer = _mRenderQueue.Renderer();

            if (!RenderQueue.supportsDrawingToScreen())
            {
                _mHelper = new CameraTextureHelper(camera, _mRenderQueue);
            }

            DrawRive(_mRenderQueue);
        }

        void DrawRive(RenderQueue queue)
        {
            if (_mArtboard == null)
            {
                return;
            }
            _mRiveRenderer.Align(fit, Alignment ?? Alignment.Center, _mArtboard);
            _mRiveRenderer.Draw(_mArtboard);
        
        }

        Vector2 _mLastMousePosition;
        bool _mWasMouseDown = false;

        void Update()
        {
            _mHelper?.UpdateTextureHelper();
            if (_mArtboard == null)
            {
                return;
            }

            Camera camera = gameObject.GetComponent<Camera>();
            if (camera is not null)
            {
                Vector3 mousePos = camera.ScreenToViewportPoint(Input.mousePosition);
                Vector2 mouseRiveScreenPos = new Vector2(
                    mousePos.x * camera.pixelWidth,
                    (1 - mousePos.y) * camera.pixelHeight
                );
                if (_mLastMousePosition != mouseRiveScreenPos)
                {
                    Vector2 local = _mArtboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, camera.pixelWidth, camera.pixelHeight),
                        fit,
                        Alignment
                    );
                    _mStateMachine?.PointerMove(local);
                    _mLastMousePosition = mouseRiveScreenPos;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 local = _mArtboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, camera.pixelWidth, camera.pixelHeight),
                        fit,
                        Alignment
                    );
                    _mStateMachine?.PointerDown(local);
                    _mWasMouseDown = true;
                }
                else if (_mWasMouseDown)
                {
                    _mWasMouseDown = false;
                    Vector2 local = _mArtboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, camera.pixelWidth, camera.pixelHeight),
                        fit,
                        Alignment
                    );
                    _mStateMachine?.PointerUp(local);
                }
            }

            // Find reported Rive events before calling advance.
            foreach (var report in _mStateMachine?.ReportedEvents() ?? Enumerable.Empty<ReportedEvent>())
            {
                OnRiveEvent?.Invoke(report);
            }

            _mStateMachine?.Advance(Time.deltaTime);
            _mRiveRenderer.Submit();
        }

        void OnDisable()
        {
            Camera camera = gameObject.GetComponent<Camera>();
            if (_mCommandBuffer is not null && camera is not null)
            {
                camera.RemoveCommandBuffer(cameraEvent, _mCommandBuffer);
            }

        }
    }
}