using System;
using Plugins.Rive.UI;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    private RiveScreen RiveScreen;


    private void Awake()
    {
        RiveScreen = GetComponent<RiveScreen>();
    }
}
