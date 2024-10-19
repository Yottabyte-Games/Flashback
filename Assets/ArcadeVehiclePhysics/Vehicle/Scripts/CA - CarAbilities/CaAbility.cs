using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities
{
    public abstract class CaAbility : MonoBehaviour
    {
        public string axisKey = "New Ability";
        public KeyCode abilityButton;
        public abstract void CheckInput();
        public abstract void DoAbility();

    }
}
