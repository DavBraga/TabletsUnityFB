using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class MenuScreen : MonoBehaviour
{
    [SerializeField]AuthHandler authHandler;
    [SerializeField] LoadingScreen loadingScreen;
    [SerializeField] TextMeshProUGUI userText;
    [SerializeField] ScreenNavigator navigator;

    [Header("LogIN/Off buttons")]
    [SerializeField] Button on; 
    [SerializeField] Button off;

    [Header("Register")]
    [SerializeField]string registerRoute = "Dados de Dispositivo";

    [SerializeField] Button registerButton;
    [Header("Admin Buttons")]
    [SerializeField]Button wipeButton;
    private void OnEnable() {

         StartCoroutine(SetUpUser());

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

    private void Start() {
        registerButton.onClick.RemoveAllListeners();
        registerButton.onClick.AddListener(
            ()=>{navigator.Navigate(registerRoute,1);}
        );
       
        
    }
    

    IEnumerator SetUpUser()
    {
        yield return new WaitUntil(()=> authHandler.AmIReady()); 
        string user = authHandler.GetCurrentUser();
        if(string.IsNullOrEmpty(user))
        {
            userText.text ="User: "+ "Visitante";
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
        }
        
        else
        {
            userText.text ="User: "+ user;
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
        }
    }

   
}
