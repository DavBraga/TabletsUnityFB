using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MenuScreen : MonoBehaviour
{
    [SerializeField]AuthHandler authHandler;
    [SerializeField] LoadingScreen loadingScreen;
    [Header("Admin Buttons")]
    [SerializeField]Button wipeButton;
    private void OnEnable() {

        if(authHandler.CheckAdmin())
        {
            EnableADMButtons();
        }
        else{
            DisableADMButtons();
        }
        loadingScreen.CloseLoadScreen();
    }

    private void DisableADMButtons()
    {
        wipeButton.gameObject.SetActive(false);
        wipeButton.interactable = false;
    }

    private void EnableADMButtons()
    {
        wipeButton.gameObject.SetActive(true);
        wipeButton.interactable = true;
    }
}
