using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DavsInputValidation;


public class SubscriberHandler : MonoBehaviour
{
    [ Header("Inputs")]
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] SerializableTMPDropdown status;
    [SerializeField] SerializableTMPDropdown paradeiro;

    [ Header("Systems")]
    [SerializeField] ScreenNavigator navigator;
    [SerializeField] CachedData cachedData;
    //tmp
     [ Header("Support")]
    [SerializeField] BdOps databaseHandler;
    [SerializeField] Alert alert;
    [SerializeField] LoadingScreen loadingScreen;
    [SerializeField] string scannerRoute = "scanner";

    private bool readFromScanner = false;
    TMP_InputField targetInpuField;


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

    private void OnEnable() {
        if(!readFromScanner){ 
            patrimonio.text = "";
            IMEI.text ="";
            return;
            }
        targetInpuField.text = cachedData.GetCachedBarCodeData();
        targetInpuField.Select();
        Validation.ValidateField(IMEI.textComponent,16,Color.red,Color.black);
        Validation.ValidateField(patrimonio.textComponent,7,Color.red,Color.black);
        targetInpuField = null;
        readFromScanner = false;
    }

    public async void SaveData()
    {
        if (string.IsNullOrEmpty(IMEI.text)||IMEI.text.Length!=15 )
        {
            alert.Open("IMEI inválido!");
            return;
        }
        if (string.IsNullOrEmpty(patrimonio.text)||patrimonio.text.Length!=6)
        {
            alert.Open("Patrimônio inválido!");
            return;
        } 

        DeviceBaseData tabletbaseData = new()
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
        navigator.Navigate(navigator.GetCurrentScreen(),0);
    }

    public void ButtonScanPatrimonio()
    {
        readFromScanner = true;
        targetInpuField = patrimonio;
        navigator.Navigate(scannerRoute,1);
        
    }
    public void ButtonScanIMEI()
    {
        readFromScanner = true;
        targetInpuField = IMEI;
        navigator.Navigate(scannerRoute,1);
        
    }
    public void ResetFields()
    {
        IMEI.text = null;
        patrimonio.text = null;
        status.Dropdown.value = 0;
        paradeiro.Dropdown.value = 0;
    }

    

}
