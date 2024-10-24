using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject indicator;

    private void Start()
    {
        Instantiate(indicator, transform);
    }
}
