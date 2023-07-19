using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Firebase.Firestore;
using System.Linq;

public class ResultHandler : MonoBehaviour
{
    //elementos do formulário
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] ScreenNavigator screenNavigator;
    [SerializeField] SerializableTMPDropdown status;
    [SerializeField] SerializableTMPDropdown paradeiro;

    [Header("Edit Buttons")]
    [SerializeField] Button salvar;
    [SerializeField] Button edit;
    [SerializeField] Button delete;
    [SerializeField] Button cadastrar;
    [SerializeField] Confirmar confirmar;
    [SerializeField] Button observation;

    [Header("UI")]
    //elementos de UI
    [SerializeField] Alert alert;
    [SerializeField] LoadingScreen loadingScreen;
    // banco de dados
    [SerializeField] BdOps databaseHandler;

    // dados recuperados
    Dictionary<string, object> data = new();
    [SerializeField] string observationsRoute="Observações";

    private void OnEnable()
    {
        //toredo
        if (screenNavigator.GetScreenMode() == 1)
        {
            OpenRegisterMode();
        }
        else OpenQueryResultMode(); 
    }

    private void OpenRegisterMode()
    {
        EnableEdit();
        salvar.gameObject.SetActive(false);
        edit.gameObject.SetActive(false);
        delete.gameObject.SetActive(false);
        observation.gameObject.SetActive(false);

        cadastrar.gameObject.SetActive(true);
    }

    private void OpenQueryResultMode()
    {
        DisableEdit();

        salvar.gameObject.SetActive(true);
        edit.gameObject.SetActive(true);
        delete.gameObject.SetActive(true);
        observation.gameObject.SetActive(true);

        cadastrar.gameObject.SetActive(false);

        if (databaseHandler.workingDeviceSnapShot == null) return;
        Debug.Log("results!");
        data = databaseHandler.workingDeviceSnapShot.ToDictionary();
        if (data.TryGetValue("patrimonio", out object obj))
        {
            patrimonio.text = obj.ToString();
        }
            
        else patrimonio.text = "error";

        if (data.TryGetValue("IMEI", out obj))
            IMEI.text = obj.ToString();
        else IMEI.text = "error";

        if (data.TryGetValue("status", out object obj2))
            status.Dropdown.value = Convert.ToInt32(obj2);
        else status.Dropdown.value = -1;


        if (data.TryGetValue("paradeiro", out obj2))
            paradeiro.Dropdown.value = Convert.ToInt32(obj2);
        else paradeiro.Dropdown.value = -1;
    }

    public void EnableEdit()
    {
        patrimonio.interactable = true;
        status.Dropdown.interactable = true;
        paradeiro.Dropdown.interactable = true;
        salvar.interactable = true;
        
        if(screenNavigator.GetScreenMode()==1) 
            IMEI.interactable = true;
        
    }

    public void DisableEdit()
    {
        patrimonio.interactable = false;
        status.Dropdown.interactable = false;
        paradeiro.Dropdown.interactable = false;
        IMEI.interactable = false;
        salvar.interactable= false;
    }

    public async void SobreEscrever()
    {
         DeviceBaseData tabletNewData = new()
        {
            IMEI = IMEI.text,
            patrimonio = patrimonio.text,
            status = status.Dropdown.value,
            paradeiro = paradeiro.Dropdown.value
        };
        loadingScreen.OpenLoadScreen();
        await databaseHandler.OverwriteDevice(databaseHandler.workingDeviceSnapShot.Id, tabletNewData);
        loadingScreen.CloseLoadScreen();
        DisableEdit();
    }

    public async void DeleteTablet()
    {
        loadingScreen.OpenLoadScreen();
        await databaseHandler.DeleteDevice(databaseHandler.workingDeviceSnapShot.Id);
        loadingScreen.CloseLoadScreen();
        alert.Open("Dispostivo excluido!");
        DisableEdit();
    }
    public void ButtonOverwrite()
    {
        confirmar.gameObject.SetActive(true);
        confirmar.CallConfirmar(SobreEscrever);
    }
    public void ButtonDelete() 
    {
        confirmar.gameObject.SetActive(true);
        confirmar.CallConfirmar(DeleteTablet);
        confirmar.onConfirm +=()=> screenNavigator.NavigateBack();
    }

    public  async void QueryObservations()
    {
        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>();
        loadingScreen.OpenLoadScreen();
        result = await databaseHandler.AsyncQueryDeviceObservations(databaseHandler.workingDocumentReference);
        loadingScreen.CloseLoadScreen();
        screenNavigator.Navigate(observationsRoute);

    }

}


