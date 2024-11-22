using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference AmbienceHubworld { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }


    [field: Header("Dialogue")]

    [field: SerializeField] public EventReference Dialogue { get; private set; }

    public static FMODEvents Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one FMODEvents instance in the scene");
        }
        Instance = this;
    }

}
