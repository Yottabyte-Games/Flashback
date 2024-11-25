using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNativePropertyTest : MonoBehaviour
    {
        [ShowNativeProperty]
        Transform Transform
        {
            get
            {
                return transform;
            }
        }

        [ShowNativeProperty]
        Transform ParentTransform
        {
            get
            {
                return transform.parent;
            }
        }

        [ShowNativeProperty]
        ushort MyUShort
        {
            get
            {
                return ushort.MaxValue;
            }
        }

        [ShowNativeProperty]
        short MyShort
        {
            get
            {
                return short.MaxValue;
            }
        }

        [ShowNativeProperty]
        ulong MyULong
        {
            get
            {
                return ulong.MaxValue;
            }
        }

        [ShowNativeProperty]
        long MyLong
        {
            get
            {
                return long.MaxValue;
            }
        }

        [ShowNativeProperty]
        uint MyUInt
        {
            get
            {
                return uint.MaxValue;
            }
        }

        [ShowNativeProperty]
        int MyInt
        {
            get
            {
                return int.MaxValue;
            }
        }
    }
}
