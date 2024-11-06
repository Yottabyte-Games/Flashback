using System.Threading.Tasks;
using UnityEngine;

namespace Utility
{
    namespace Methods
    {
        public static class UMethods
        {
            /// <summary>
            /// Resets the transform's position, rotation and size.
            /// </summary>
            /// <param name="transform">the transform to reset</param>
            /// <param name="isLocal">should it be done locally</param>
            /// <param name="keepPosition">do we keep the position?</param>
            /// <param name="keepRotation">do we keep the rotation?</param>
            /// <param name="keepSize">do we keep the size?</param>
            public static void ResetTransform(Transform transform, bool isLocal = false, bool keepPosition = false, bool keepRotation = false, bool keepSize = true)
            {
                if (isLocal)
                {
                    if (!keepPosition) transform.localPosition = Vector3.zero;
                    if (!keepRotation) transform.localEulerAngles = Vector3.zero;
                    if (!keepSize) transform.localScale = Vector3.one;
                }
                else
                {
                    if (!keepPosition) transform.position = Vector3.zero;
                    if (!keepRotation) transform.eulerAngles = Vector3.zero;
                    if (!keepSize) transform.localScale = Vector3.one;
                }
            }
        }
    }

    namespace Math
    {
        public static class UMath
        {
            /// <summary>
            /// Linearly interpolates between two Vector3's.
            /// </summary>
            /// <param name="v1">The start value.</param>
            /// <param name="v2">The end value.</param>
            /// <param name="t">The interpolation value between the two floats.</param>
            /// <returns>The interpolated vector3 result between the two vector3 values.</returns>
            public static Vector3 LerpVector3(Vector3 v1, Vector3 v2, float t)
            {
                return new Vector3(
                    Mathf.Lerp(v1.x, v2.x, t), 
                    Mathf.Lerp(v1.y, v2.y, t), 
                    Mathf.Lerp(v1.z, v2.z, t));
            }
        }
    }

    namespace Physics
    {
        public class UPhysics
        {
            static float Gravity
            {
                get { return -UnityEngine.Physics.gravity.y; }
            }

            /// <summary>
            /// Thow an object to a position
            /// </summary>
            /// <param name="toThrow">Object to throw</param>
            /// <param name="pos">Where to throw the object</param>
            /// <param name="time">travel time in seconds</param>
            public static void ThrowTo(Rigidbody toThrow, Vector3 pos, float time = 1)
            {
                toThrow.linearVelocity = CalculateThrow(toThrow.transform.position, pos, time);
            }
            /// <summary>
            /// Thow an object to a position, but waits until the time has passed
            /// </summary>
            /// <param name="toThrow">Object to throw</param>
            /// <param name="pos">Where to throw the object</param>
            /// <param name="time">travel time in milliseconds</param>
            public static async Task ThrowToAsync(Rigidbody toThrow, Vector3 pos, int time = 1000)
            {
                ThrowTo(toThrow, pos, time / 1000f);

                await Task.Delay(time);
            }

            static Vector3 CalculateThrow(Vector3 startPos, Vector3 endPos, float time)
            {
                float yForce = endPos.y - startPos.y + 0.5f * Gravity * time;
                float xForce = (endPos.x - startPos.x) / time;
                float zForce = (endPos.z - startPos.z) / time;

                return new Vector3(xForce, yForce, zForce);
            }
        }
    }
}
