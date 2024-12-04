using System.Collections;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ButtonTest : MonoBehaviour
    {
        public int myInt;

        [Button(enabledMode: EButtonEnableMode.Always)]
        void IncrementMyInt()
        {
            myInt++;
        }

        [Button("Decrement My Int", EButtonEnableMode.Editor)]
        void DecrementMyInt()
        {
            myInt--;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        void LogMyInt(string prefix = "MyInt = ")
        {
            Debug.Log(prefix + myInt);
        }

        [Button("StartCoroutine")]
        IEnumerator IncrementMyIntCoroutine()
        {
            int seconds = 5;
            for (int i = 0; i < seconds; i++)
            {
                myInt++;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
