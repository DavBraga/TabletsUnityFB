using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavBar : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI screenTitle;
    [SerializeField] ScreenNavigator navigator;
    [SerializeField] Button returnButton;
    // Start is called before the first frame update
    void Awake()
    {
        if(navigator)
        returnButton.onClick.AddListener(()=>{navigator.NavigateBack();});
    }
    private void OnEnable() {
        if(navigator)
        navigator.onScreenTransit+=UpdateScreenName;
    }
    private void OnDisable() {
        if(navigator)
        navigator.onScreenTransit-=UpdateScreenName;
    }

    public void UpdateScreenName()
    {
        if(screenTitle)
        screenTitle.text = navigator.GetCurrentScreen();
    }
}
