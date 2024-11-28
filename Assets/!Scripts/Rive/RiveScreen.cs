using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rive;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Renderer = Rive.Renderer;
using RenderQueue = Rive.RenderQueue;
namespace _Scripts.Rive
{
    internal class CameraTextureHelper
    {
        int _pixelWidth = -1;
        int _pixelHeight = -1;
        RenderQueue _renderQueue;

        // Queue to keep things on the main thread only.
        static ConcurrentQueue<Action> _mainThreadActions = new ConcurrentQueue<Action>();

        public RenderTexture RenderTexture
        {
            get;
            private set;
        }

        public Camera MainCamera
        {
            get;
        }

        internal CameraTextureHelper(Camera camera, RenderQueue queue)
        {
            MainCamera = camera;
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
            RenderTexture?.Release();
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

            if (_pixelWidth == MainCamera.pixelWidth && _pixelHeight == MainCamera.pixelHeight)
            {
                return false;
            }
    
            Cleanup();

            _pixelWidth = MainCamera.pixelWidth;
            _pixelHeight = MainCamera.pixelHeight;
            var textureDescriptor = TextureHelper.Descriptor(_pixelWidth, _pixelHeight);
            RenderTexture = new RenderTexture(textureDescriptor);
            RenderTexture.Create();
            _renderQueue.UpdateTexture(RenderTexture);

            return true;
        }

    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
// Draw a Rive artboard to the screen. Must be bound to a camera.
    public class RiveScreen : MonoBehaviour
    {
        public enum RiveScenes
        {
            HUD,
            MainMenu,
            MiniGameSelectMenu,
            PauseMenu,
            SettingsMenu,
            PsychologyScene,
        }
        static readonly Dictionary<RiveScenes, string> ReferenceNames = new Dictionary<RiveScenes, string>()
        {
            { RiveScenes.HUD, "HUD" },
            { RiveScenes.MainMenu, "Home Screen" },
            { RiveScenes.MiniGameSelectMenu, "Mini Games Select Menu" },
            { RiveScenes.PauseMenu, "Pause Menu" },
            { RiveScenes.SettingsMenu, "Settings Menu" },
            { RiveScenes.PsychologyScene, "Psychologist MindScene"},
        };
        [FormerlySerializedAs("currentScene")] public RiveScenes CurrentScene;
        List<RiveScenes> scenesLoaded;

        public enum TextPath
        {
            HUDItem,
            Dialogue,
            Psychologist,
            Option1,
            Option2,
        }

        readonly Dictionary<TextPath, string> _textRunReferences = new()
        {
            { TextPath.HUDItem, "ItemName"},
            { TextPath.Dialogue, "DialogueText"},
            { TextPath.Psychologist, "Psychologist Text Run" },
            { TextPath.Option1, "Option 1 Text Run" },
            { TextPath.Option2, "Option 2 Text Run" }
        };
    
        [FormerlySerializedAs("asset")] public Asset Asset;
        [FormerlySerializedAs("cameraEvent")] public CameraEvent CameraEvent = CameraEvent.AfterEverything;
        [FormerlySerializedAs("fit")] public Fit Fit = Fit.Contain;
        public Alignment Alignment = Alignment.Center;
        public event RiveEventDelegate OnRiveEvent;
        public delegate void RiveEventDelegate(ReportedEvent reportedEvent);

        RenderQueue _renderQueue;
        Renderer _riveRenderer;
        CommandBuffer _commandBuffer;

        File _file;
        Artboard _artboard;
        CameraTextureHelper _helper;

        public Artboard Artboard => _artboard;
        public StateMachine StateMachine { get; private set; }
    
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
            var texture = _helper.RenderTexture;

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
            scenesLoaded = new List<RiveScenes>();
            LoadSceneMode(CurrentScene);
        }

        public void LoadSceneMode(RiveScenes scenes)
        {
            scenesLoaded.Add(scenes);
            SetRiveScene(scenes);
        }
        public void ReturnToOriginalScene()
        {
            var previousScene = scenesLoaded[^2];
            scenesLoaded.RemoveAt(scenesLoaded.Count - 1);
            SetRiveScene(previousScene);
        }
        private void SetRiveScene(RiveScenes scenes)
        {
            if (Asset is not null)
            {
                _file = File.Load(Asset);
                _artboard = _file.Artboard(GetSelectedRiveSceneName(scenes));
                StateMachine = _artboard?.StateMachine();
                CurrentScene = scenes;
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
            _riveRenderer.Align(Fit, Alignment ?? Alignment.Center, _artboard);
            _riveRenderer.Draw(_artboard);
    
        }

        Vector2 _mLastMousePosition;
   
        Camera _mainCamera;

        void Update()
        {
            _helper?.UpdateTextureHelper();
            if (_artboard is null)
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
                        Fit,
                        Alignment
                    );
                    StateMachine?.PointerMove(local);
                    _mLastMousePosition = mouseRiveScreenPos;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 local = _artboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, _mainCamera.pixelWidth, _mainCamera.pixelHeight),
                        Fit,
                        Alignment
                    );
                    StateMachine?.PointerDown(local);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector2 local = _artboard.LocalCoordinate(
                        mouseRiveScreenPos,
                        new Rect(0, 0, _mainCamera.pixelWidth, _mainCamera.pixelHeight),
                        Fit,
                        Alignment
                    );
                    StateMachine?.PointerUp(local);
                }
            }

            // Find reported Rive events before calling advance.
            foreach (var report in StateMachine?.ReportedEvents() ?? Enumerable.Empty<ReportedEvent>())
            {
                OnRiveEvent?.Invoke(report);
            }

            StateMachine?.Advance(Time.deltaTime);
            _riveRenderer.Submit();
            GL.InvalidateState();
        }

        void OnDisable()
        {
            if (_commandBuffer is not null && _mainCamera is not null)
            {
                _mainCamera.RemoveCommandBuffer(CameraEvent, _commandBuffer);
            }

        }

        string GetSelectedRiveSceneName(RiveScenes swapTo)
        {
            return ReferenceNames[swapTo];
        }
    
        public void SetTextRunAtPath(string textRun, TextPath path)
        {
            if (_textRunReferences.TryGetValue(path, out var stringPath))
            {
                Artboard.SetTextRun(stringPath, textRun);
            }
            else
            {
                Debug.LogError($"Invalid text target: {path}");
            }
        }
    }
}