using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Animator animator;
    CanvasGroup canvasGroup;
    bool isOpen = false;

    //Delay precisa ser do tamanho da duração da animação de fadeOff;
    [SerializeField] float delay = 0.1f;
    private void Awake() {

        if(!animator) animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void CloseLoadScreen()
    {
        if(!isOpen) return;

        isOpen = false;
        animator.SetTrigger("tFadeOff");
        StartCoroutine(DisablAafterSecs());
        
    }

    public void OpenLoadScreen()
    {
        if(isOpen) return;
        
        isOpen = true;
        animator.SetTrigger("tFadeIn");
        canvasGroup.blocksRaycasts = true;
       
    }
    IEnumerator DisablAafterSecs()
    {
        yield return new WaitForSeconds(delay);
        canvasGroup.blocksRaycasts = false;
    }

}
