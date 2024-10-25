using UnityEngine;

namespace _Scripts.Vehicle.CA_CarAbilities
{
    public abstract class CaAbility : MonoBehaviour
    {
        public string axisKey = "New Ability";
        public KeyCode abilityButton;
        public abstract void CheckInput();
        public abstract void DoAbility();

    }
}
