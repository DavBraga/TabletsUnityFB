using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] ScreenNavigator screenNavigator;
    [SerializeField] SerializableTMPDropdown status;
    [SerializeField] SerializableTMPDropdown paradeiro;
    [SerializeField] Button salvar;
    [SerializeField] Confirmar confirmar;
    [SerializeField] Alert alert;

    [SerializeField] BdOps databaseHandler;

    Dictionary<string, object> data = new();
    private void OnEnable()
    {
        Debug.Log("LOG");
        if (databaseHandler.tabletData == null) return;
        Debug.Log("LOG1");
        data = databaseHandler.tabletData.ToDictionary();
        data.TryGetValue("patrimonio", out object obj);
        patrimonio.text = obj.ToString();

        data.TryGetValue("IMEI", out obj);
        IMEI.text = obj.ToString();

        data.TryGetValue("status", out object obj2);
        status.Dropdown.value = Convert.ToInt32(obj2);

        data.TryGetValue("paradeiro", out obj2);
        paradeiro.Dropdown.value = Convert.ToInt32(obj2);
    }

    public void EnableEdit()
    {
        patrimonio.interactable = true;
        status.Dropdown.interactable = true;
        paradeiro.Dropdown.interactable = true;
        salvar.interactable = true;
    }

    public void DisableEditEdit()
    {
        patrimonio.interactable = false;
        IMEI.interactable = false;
        status.Dropdown.interactable = false;
        paradeiro.Dropdown.interactable = false;
        salvar.interactable= false;
    }

    public void SobreEscrever()
    {
         TabletbaseData tabletNewData = new()
        {
            IMEI = IMEI.text,
            patrimonio = patrimonio.text,
            status = status.Dropdown.value,
            paradeiro = paradeiro.Dropdown.value
        };

        databaseHandler.SobreEscreverTablet(databaseHandler.tabletData.Id, tabletNewData);
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
        confirmar.confirm +=()=> screenNavigator.NavigateBack();
    }
    public void DeleteTablet()
    {
        databaseHandler.ExcluirTablet(databaseHandler.tabletData.Id);
        alert.Call("Tablet excluido!");

    }
}


