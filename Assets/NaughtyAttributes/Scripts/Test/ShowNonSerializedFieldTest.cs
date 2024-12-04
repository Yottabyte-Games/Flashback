using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNonSerializedFieldTest : MonoBehaviour
    {
#pragma warning disable 414
        [ShowNonSerializedField] ushort myUShort = ushort.MaxValue;

        [ShowNonSerializedField] short myShort = short.MaxValue;

        [ShowNonSerializedField] uint myUInt = uint.MaxValue;

        [ShowNonSerializedField] int myInt = 10;

        [ShowNonSerializedField] ulong myULong = ulong.MaxValue;

        [ShowNonSerializedField] long myLong = long.MaxValue;

        [ShowNonSerializedField] const float PI = 3.14159f;

        [ShowNonSerializedField] static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414
    }
}
