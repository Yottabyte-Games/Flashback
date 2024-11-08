using UnityEngine;

namespace GitAmendKartSample.Scripts {
    public interface IDrive {
        Vector2 Move { get; }
        bool IsBraking { get; }
        void Enable();
    }
}