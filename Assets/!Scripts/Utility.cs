using System.Threading.Tasks;
using UnityEngine;

namespace Utility.Math
{
    public static class UMath
    {
        public static Vector3 LerpVector3(Vector3 v1, Vector3 v2, float t)
        {
            return new Vector3(Mathf.Lerp(v1.x, v2.x, t), Mathf.Lerp(v1.y, v2.y, t), Mathf.Lerp(v1.z, v2.z, t));
        }
    }
}
namespace Utility.Physics
{
    public class UPhysics
    {
        static float gravity
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
            toThrow.linearVelocity = CalculateThrow(toThrow.transform.position, pos, time) * toThrow.mass;
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
            float yForce = endPos.y - startPos.y + 0.5f * gravity * time;
            float xForce = (endPos.x - startPos.x) / time;
            float zForce = (endPos.z - startPos.z) / time;

            return new Vector3(xForce, yForce, zForce);
        }
    }
}
