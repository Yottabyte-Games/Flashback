using UnityEngine;

namespace _Scripts
{
    public static class UMath
    {
        public static Vector3 LerpVector3(Vector3 v1, Vector3 v2, float t)
        {
            return new Vector3(Mathf.Lerp(v1.x, v2.x, t), Mathf.Lerp(v1.y, v2.y, t), Mathf.Lerp(v1.z, v2.z, t));
        }
    }
}
