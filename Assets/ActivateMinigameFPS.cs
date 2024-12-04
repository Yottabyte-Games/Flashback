using UnityEngine;

public class ActivateMinigameFPS : MonoBehaviour
{
    [SerializeField] GameObject catWeapon;

    void Start()
    {
        catWeapon.SetActive(false);
    }
    
    public void ActivateMinigame()
    {
        catWeapon.SetActive(true);
    }
}
