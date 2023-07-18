using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenNewObservation : MonoBehaviour
{
    [SerializeField] TMP_InputField observationTitle;
    [SerializeField] TMP_InputField observationContent;
    [SerializeField] SerializableTMPDropdown type;
    [SerializeField] GameObject typeContainer;

    [Header("Systems")]
    [SerializeField]BdOps database;
    [SerializeField] ScreenNavigator navigator;

    [Header("UI")]
    [SerializeField] LoadingScreen loadingScreen;
    private Dictionary<string, object> data;

    // Start is called before the first frame update

    private void OnEnable() {
        if(navigator.GetScreenMode()==1) OpenInCreationMode();
        else OpenInReadMode();
    }
    public void OpenInCreationMode()
    {
        observationTitle.text = "";
        observationContent.text = "";
        type.Dropdown.value = 0;

        observationTitle.interactable = true;
        observationContent.interactable = true;
        typeContainer.SetActive(true);
        type.Dropdown.gameObject.SetActive(true);
    }

    public void OpenInReadMode()
    {
        
        observationTitle.interactable = false;
        observationContent.interactable = false;
        typeContainer.SetActive(false);
        type.Dropdown.gameObject.SetActive(false);

        if (database.workingDocumentReference == null) return;
        data = database.observationSnapshot.ToDictionary();
        if (data.TryGetValue("title", out object obj))
            observationTitle.text = obj.ToString();
        else observationTitle.text = "error";

        if (data.TryGetValue("content", out obj))
            observationContent.text = obj.ToString();
        else observationContent.text = "error";

        // if (data.TryGetValue("tag", out object obj2))
        //     type.Dropdown.value = Convert.ToInt32(obj2);
        // else status.Dropdown.value = -1;


        // if (data.TryGetValue("paradeiro", out obj2))
        //     paradeiro.Dropdown.value = Convert.ToInt32(obj2);
        // else paradeiro.Dropdown.value = -1;
    }
    public async void RegisterObservation()
    {
        ObservationData observationData = new() 
        {
            title = observationTitle.text,
            content = observationContent.text,
            tag = type.Dropdown.value,
        };

        loadingScreen.OpenLoadScreen();
        await database.RegisterObservation(observationData, database.workingDocumentReference);
        loadingScreen.CloseLoadScreen();
        navigator.NavigateBack();
    }
}
