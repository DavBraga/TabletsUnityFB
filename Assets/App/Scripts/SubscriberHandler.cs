using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubscriberHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] SerializableTMPDropdown status;
    [SerializeField] SerializableTMPDropdown paradeiro;
    //tmp

    [SerializeField] BdOps databaseHandler;
    [SerializeField] Alert alert;

    public void SaveData()
    {
        if (databaseHandler.querying) { Debug.Log("DB is busy..."); return; }
        if (string.IsNullOrEmpty(IMEI.text)) return;
        if (string.IsNullOrEmpty(patrimonio.text)) return;


        TabletbaseData tabletbaseData = new()
        {
            IMEI = IMEI.text,
            patrimonio = patrimonio.text,
            status = status.Dropdown.value,
            paradeiro = paradeiro.Dropdown.value
        };

        databaseHandler.CatalogarTablet(tabletbaseData);
        alert.gameObject.SetActive(true);
        alert.Call("Tablet cadastrado!");
    }

}
