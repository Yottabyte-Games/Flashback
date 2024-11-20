using Plugins.Dreamteck.Splines.Core;
using UnityEngine;

namespace Plugins.Dreamteck.Splines.Components
{
    public interface ISampleModifier
    {
        public void ApplySampleModifiers(ref SplineSample sample);

        public Vector3 GetModifiedSamplePosition(ref SplineSample sample);
    }
}
