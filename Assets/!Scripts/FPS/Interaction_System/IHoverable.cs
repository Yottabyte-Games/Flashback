using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{    
    public interface IHoverable
    {
        string Tooltip { get; set;}
        Transform TooltipTransform { get; }

        void OnHoverStart(Material _hoverMat);
        void OnHoverEnd();
    }
}
