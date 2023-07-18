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

    [SerializeField]string home = "home";
    bool isActive = false;

    Animator navbarAnimator;
    Coroutine disableroutine;
    // Start is called before the first frame update
    void Awake()
    {
        if(navigator)
        returnButton.onClick.AddListener(()=>{navigator.NavigateBack();});
        navbarAnimator = GetComponent<Animator>();
    }
    private void OnEnable() {
        if(navigator)
        navigator.onScreenTransit+=UpdateScreenName;
        //Arrive();
    }
    private void OnDisable() {
        if(navigator)
        navigator.onScreenTransit-=UpdateScreenName;
    }

    public void UpdateScreenName()
    {
        Evaluate();
        if(screenTitle)
        screenTitle.text = navigator.GetCurrentScreen();

    }

    public void Arrive()
    {
        isActive = true;
        navbarAnimator.SetTrigger("arrive");
    }
     public void Leave()
    {
        isActive = false;
        navbarAnimator.SetTrigger("leave");
    }
    public void Disable()
    {
        Leave();
    }

    public void Evaluate()
    {
        if(isActive && navigator.GetCurrentScreen()==home) Leave();
        if(!isActive&& navigator.GetCurrentScreen()!=home) Arrive();
    }
}
