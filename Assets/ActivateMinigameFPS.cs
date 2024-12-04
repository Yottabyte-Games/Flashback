using UnityEngine;
using System.Collections;

public class ActivateMinigameFPS : MonoBehaviour
{
    [SerializeField] GameObject catWeapon;


    void Start()
    {
        catWeapon.SetActive(false);
    }

    void Update()
    {
        ActivateMinigame();
    }
    
    public void ActivateMinigame()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
           catWeapon.SetActive(true);
    }
}
