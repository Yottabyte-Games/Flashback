using _Scripts.FPS.Scriptable_Objects;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.FPS.Camera_Controller
{    
    public class CameraController : MonoBehaviour
    {
        #region Variables
            #region Data
                [Space,Header("Data")]
                [SerializeField]
                CameraInputData camInputData = null;

                [Space,Header("Custom Classes")]
                [SerializeField]
                CameraZoom cameraZoom = null;
                [SerializeField] CameraSwaying cameraSway = null;

            #endregion

            #region Settings
                [Space,Header("Look Settings")]
                [SerializeField]
                Vector2 sensitivity = Vector2.zero;
                [SerializeField] Vector2 smoothAmount = Vector2.zero;
                [SerializeField] [MinMaxSlider(-90f,90f)]
                Vector2 lookAngleMinMax = Vector2.zero;
            #endregion

            #region Private

            float m_yaw;
            float m_pitch;

            float m_desiredYaw;
            float m_desiredPitch;

                #region Components

                Transform m_pitchTranform;
                Camera m_cam;
                #endregion
            #endregion
            
        #endregion

        #region BuiltIn Methods  
            void Awake()
            {
                GetComponents();
                InitValues();
                InitComponents();
                ChangeCursorState();
            }

            void LateUpdate()
            {
                CalculateRotation();
                SmoothRotation();
                ApplyRotation();
                HandleZoom();
            }
        #endregion

        #region Custom Methods
            void GetComponents()
            {
                m_pitchTranform = transform.GetChild(0).transform;
                m_cam = GetComponentInChildren<Camera>();
            }

            void InitValues()
            {
                m_yaw = transform.eulerAngles.y;
                m_desiredYaw = m_yaw;
            }

            void InitComponents()
            {
                cameraZoom.Init(m_cam, camInputData);
                cameraSway.Init(m_cam.transform);
            }

            void CalculateRotation()
            {
                m_desiredYaw += camInputData.InputVector.x * sensitivity.x * Time.deltaTime;
                m_desiredPitch -= camInputData.InputVector.y * sensitivity.y * Time.deltaTime;

                m_desiredPitch = Mathf.Clamp(m_desiredPitch,lookAngleMinMax.x,lookAngleMinMax.y);
            }

            void SmoothRotation()
            {
                m_yaw = Mathf.Lerp(m_yaw,m_desiredYaw, smoothAmount.x * Time.deltaTime);
                m_pitch = Mathf.Lerp(m_pitch,m_desiredPitch, smoothAmount.y * Time.deltaTime);
            }

            void ApplyRotation()
            {
                transform.eulerAngles = new Vector3(0f,m_yaw,0f);
                m_pitchTranform.localEulerAngles = new Vector3(m_pitch,0f,0f);
            }

            public void HandleSway(Vector3 _inputVector,float _rawXInput)
            {
                cameraSway.SwayPlayer(_inputVector,_rawXInput);
            }

            void HandleZoom()
            {
                if(camInputData.ZoomClicked || camInputData.ZoomReleased)
                    cameraZoom.ChangeFOV(this);

            }

            public void ChangeRunFOV(bool _returning)
            {
                cameraZoom.ChangeRunFOV(_returning,this);
            }

            void ChangeCursorState()
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        #endregion
    }
}
