using UnityEngine;

namespace YottabyteGames.Scripts.Interaction_System
{    
    public interface IHoverable
    {
        string Tooltip { get; set;}
        Transform TooltipTransform { get; }

        void OnHoverStart(Material _hoverMat);
        void OnHoverEnd();
    }
}
