using System;
using System.Collections.Concurrent;
using System.Linq;
using Rive;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using Renderer = Rive.Renderer;
using RenderQueue = Rive.RenderQueue;

namespace Plugins.Rive.UI
{
    
    internal class CameraTextureHelper
    {
        Camera _mainCamera;
        RenderTexture _renderTexture;
        int _pixelWidth = -1;
        int _pixelHeight = -1;
        RenderQueue _renderQueue;

        // Queue to keep things on the main thread only.
        static ConcurrentQueue<Action> _mainThreadActions = new ConcurrentQueue<Action>();

        public RenderTexture renderTexture => _renderTexture;

        public Camera MainCamera => _mainCamera;

        internal CameraTextureHelper(Camera camera, RenderQueue queue)
        {
            _mainCamera = camera;
            _renderQueue = queue;
            UpdateTextureHelper();
        }

        ~CameraTextureHelper()
        {
            // Since the GC calls the destructor and doesn't run on the main thread,
            // we need to ensure the cleanup() call happens on the main thread.
            _mainThreadActions.Enqueue(Cleanup);
        }

        void Cleanup()
        {
            _renderTexture?.Release();
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

            if (_pixelWidth == _mainCamera.pixelWidth && _pixelHeight == _mainCamera.pixelHeight)
            {
                return false;
            }
        
            Cleanup();

            _pixelWidth = _mainCamera.pixelWidth;
            _pixelHeight = _mainCamera.pixelHeight;
            var textureDescriptor = TextureHelper.Descriptor(_pixelWidth, _pixelHeight);
            _renderTexture = new RenderTexture(textureDescriptor);
            _renderTexture.Create();
            _renderQueue.UpdateTexture(_renderTexture);

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
        public Alignment alignment = Alignment.Center;
        public event RiveEventDelegate OnRiveEvent;
        public delegate void RiveEventDelegate(ReportedEvent reportedEvent);

        RenderQueue _renderQueue;
        Renderer _riveRenderer;
        CommandBuffer _commandBuffer;

        File _file;
        Artboard _artboard;
        CameraTextureHelper _helper;

        public StateMachine stateMachine { get; private set; }

        void Start()
        {
            _mainCamera = gameObject.GetComponent<Camera>();
        }

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
            if (_helper is null || !Event.current.type.Equals(EventType.Repaint)) return;
            var texture = _helper.renderTexture;

            var width = _helper.MainCamera.scaledPixelWidth;
            var height = _helper.MainCamera.scaledPixelHeight;

            GUI.DrawTexture(
                FlipY() ? new Rect(0, height, width, -height) : new Rect(0, 0, width, height),
                texture,
                ScaleMode.StretchToFill,
                true
            );
        }

        void Awake()
        {
            if (asset is not null)
            {
                _file = File.Load(asset);
                _artboard = _file.Artboard(0);
                stateMachine = _artboard?.StateMachine();
            }

            Camera mainCamera = gameObject.GetComponent<Camera>();
            Assert.IsNotNull(mainCamera, "RiveScreen must be attached to a camera.");

            // Make a RenderQueue that doesn't have a backing texture and does not
            // clear the target (we'll be drawing on top of it).
            _renderQueue = new RenderQueue(null, false);
            _riveRenderer = _renderQueue.Renderer();

            if (!RenderQueue.supportsDrawingToScreen())
            {
                _helper = new CameraTextureHelper(mainCamera, _renderQueue);
            }

            DrawRive(_renderQueue);
        }

        void DrawRive(RenderQueue queue)
        {
            if (_artboard is null)
            {
                return;
            }
            _riveRenderer.Align(fit, alignment ?? Alignment.Center, _artboard);
            _riveRenderer.Draw(_artboard);
        
        }

        Vector2 _mLastMousePosition;
        bool _wasMouseDown;
        Camera _mainCamera;

        void Update()
        {
            _helper?.UpdateTextureHelper();
            if (_artboard == null)
            {
                return;
            }

            if (_mainCamera is not null)
            {
                Vector3 mousePos = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
                Vector2 mouseRiveScreenPos = new Vector2(
                    mousePos.x * _mainCamera.pixelWidth,
                    (1 - mousePos.y) * _mainCamera.pixelHeight
                );
                if (!_mLastMousePosition.Equals(mouseRiveScreenPos))
                {
                    Vector2 local = _artboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, _mainCamera.pixelWidth, _mainCamera.pixelHeight),
                        fit,
                        alignment
                    );
                    stateMachine?.PointerMove(local);
                    _mLastMousePosition = mouseRiveScreenPos;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 local = _artboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, _mainCamera.pixelWidth, _mainCamera.pixelHeight),
                        fit,
                        alignment
                    );
                    stateMachine?.PointerDown(local);
                    _wasMouseDown = true;
                }
                else if (_wasMouseDown)
                {
                    _wasMouseDown = false;
                    Vector2 local = _artboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, _mainCamera.pixelWidth, _mainCamera.pixelHeight),
                        fit,
                        alignment
                    );
                    stateMachine?.PointerUp(local);
                }
            }

            // Find reported Rive events before calling advance.
            foreach (var report in stateMachine?.ReportedEvents() ?? Enumerable.Empty<ReportedEvent>())
            {
                OnRiveEvent?.Invoke(report);
            }

            stateMachine?.Advance(Time.deltaTime);
            _riveRenderer.Submit();
        }

        void OnDisable()
        {
            if (_commandBuffer is not null && _mainCamera is not null)
            {
                _mainCamera.RemoveCommandBuffer(cameraEvent, _commandBuffer);
            }

        }
        public void SetDialogue(string dialogueString)
        {
            _artboard.SetTextRun("DialogueText", dialogueString);
        }

        public void SetHoverItemName(string objectName)
        {
            _artboard.SetTextRun("ItemName", objectName);
        }
    }
}