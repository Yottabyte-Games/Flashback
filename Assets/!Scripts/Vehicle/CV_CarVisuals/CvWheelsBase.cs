using System.Collections.Generic;
using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvWheelsBase : MonoBehaviour
    {

        [SerializeField]
        GameObject wheelPrefab;
        [SerializeField]
        Transform[] wheelAttachPoints;    //Different to the placement of physics wheels
        [SerializeField]
        List<CvWheels> wheelVisuals = new List<CvWheels>();

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = transform.parent.GetComponentInChildren<CpMain>();
        
        }
    
        // Start is called before the first frame update
        void Start()
        {
            InitWheels();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            foreach (var wheelVisual in wheelVisuals)
            {
                wheelVisual.ProcessWheelVisuals(_cpMain.input, _cpMain.speedData);
            }
        }

        void InitWheels()
        {
            foreach (var wheelAttachPoint in wheelAttachPoints)
            {
                var wheelVisual = Instantiate(wheelPrefab, wheelAttachPoint.position, wheelAttachPoint.rotation, wheelAttachPoint).GetComponent<CvWheels>();
                wheelVisual.SetUpWheel(_cpMain.rb);
                wheelVisuals.Add(wheelVisual);
            }
        }
    }
}
