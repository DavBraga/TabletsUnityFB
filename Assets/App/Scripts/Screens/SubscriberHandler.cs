using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DavsInputValidation;


public class SubscriberHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] SerializableTMPDropdown status;
    [SerializeField] SerializableTMPDropdown paradeiro;
    //tmp

    [SerializeField] BdOps databaseHandler;
    [SerializeField] Alert alert;
    [SerializeField] LoadingScreen loadingScreen;


    private  void Start() 
    {
        IMEI.onDeselect.AddListener(
            (string a)=>{
                Validation.ValidateField(IMEI.textComponent,16,Color.red,Color.black);
            });

        IMEI.onSelect.AddListener(
            (string a)=>{
                IMEI.textComponent.color = Color.black;
            });

        patrimonio.onDeselect.AddListener(
            (string a)=>{
                Validation.ValidateField(patrimonio.textComponent,7,Color.red,Color.black);
            });
            
        patrimonio.onSelect.AddListener(
            (string a)=>{
                patrimonio.textComponent.color = Color.black;
            });

    }

    public async void SaveData()
    {
        if (string.IsNullOrEmpty(IMEI.text)||IMEI.text.Length<15 )
        {
            alert.Open("IMEI inválido!");
            return;
        }
        if (string.IsNullOrEmpty(patrimonio.text)||patrimonio.text.Length<6)
        {
            alert.Open("Patrimônio inválido!");
            return;
        } 

        TabletbaseData tabletbaseData = new()
        {
            IMEI = IMEI.text,
            patrimonio = patrimonio.text,
            status = status.Dropdown.value,
            paradeiro = paradeiro.Dropdown.value
        };

        loadingScreen.OpenLoadScreen();
        await databaseHandler.TryRegisterDevice(tabletbaseData);
        loadingScreen.CloseLoadScreen();
        ResetFields();
    }

    public void ResetFields()
    {
        IMEI.text = null;
        patrimonio.text = null;
        status.Dropdown.value = 0;
        paradeiro.Dropdown.value = 0;
    }

    

}
