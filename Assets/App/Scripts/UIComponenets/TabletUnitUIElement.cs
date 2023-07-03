using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TabletUnitUIElement : MonoBehaviour
{
    // elementos de UI
    [SerializeField]TextMeshProUGUI index;
     [SerializeField]TextMeshProUGUI  chaveIMEIPatri, status, paradeiro;

    string patri="", imei="";
    public Button button;

    Dictionary<string, object> data = new();

    public void Initialize(int indexNumber, DocumentSnapshot documentSnapshot, BdOps database, UnityAction followedAction)
    {
        data = documentSnapshot.ToDictionary();
        index.text = indexNumber.ToString();
        if(data.TryGetValue("patrimonio", out object obj))
        patri = chaveIMEIPatri.text = obj.ToString();
        else  chaveIMEIPatri.text = "error";

         if(data.TryGetValue("IMEI", out obj)) 
         imei =obj.ToString();
         else  chaveIMEIPatri.text = "error";

        if(data.TryGetValue("status", out object obj2))
        status.text = GlobalData.ConvertStatus(Convert.ToInt32(obj2));
        else status.text = "error";
        

        if(data.TryGetValue("paradeiro", out obj2))
        paradeiro.text =  GlobalData.ConvertParadeiro(Convert.ToInt32(obj2));
        else paradeiro.text = "error";


        button.onClick.AddListener(()=>
        {
            database.UpdateWorkingTabletSnapshot(documentSnapshot);
        }
        );
        button.onClick.AddListener(followedAction);

    }

    private void OnDisable() {
        button.onClick.RemoveAllListeners();
    }
}
