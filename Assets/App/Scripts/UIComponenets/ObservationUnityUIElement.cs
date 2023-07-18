using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Firestore;
using System;
using UnityEngine.Events;

public class ObservationUnityUIElement : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI index;
    [SerializeField]TextMeshProUGUI  title, timestamp;
    Image background;
    int observationTag = 0;

    string titleString="";
    public Button button;

    Dictionary<string, object> data = new();


    public void Initialize(int indexNumber, DocumentSnapshot documentSnapshot, BdOps database, UnityAction followedAction)
    {
        button.onClick.RemoveAllListeners();
        data = documentSnapshot.ToDictionary();
        index.text = indexNumber.ToString();
        if (data.TryGetValue("title", out object obj))
            titleString = title.text = obj.ToString();
        else title.text = "error";

        if (data.TryGetValue("tag", out object obj2))
            observationTag = Convert.ToInt32(obj2);
        Debug.Log("observationTag: " + observationTag);
        background = GetComponent<Image>();
        SetBGColor();

        if (data.TryGetValue("timestamp", out object obj3))
        {
            if (obj3 is Firebase.Firestore.Timestamp firestoreTimestamp)
            {
                System.DateTime unityDateTime = firestoreTimestamp.ToDateTime();
                timestamp.text = unityDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            }
        }


        button.onClick.AddListener(() =>
        {
            database.UpdateWorkingObservationSnapshot(documentSnapshot);
        }
        );
        button.onClick.AddListener(followedAction);

    }

    private void SetBGColor()
    {
        Color newBgColor = new();
        if (observationTag == 0)
            ColorUtility.TryParseHtmlString(GlobalData.neutralColor, out newBgColor);
        else if(observationTag == 1)
            ColorUtility.TryParseHtmlString(GlobalData.goodColor, out newBgColor);
        else if( observationTag ==2)
            ColorUtility.TryParseHtmlString(GlobalData.alertColor, out newBgColor);
        else if( observationTag ==3)
            ColorUtility.TryParseHtmlString(GlobalData.problemColor, out newBgColor);

        Debug.Log("Color set to:"+ newBgColor.ToString());

        background.color = newBgColor;
    }
}
