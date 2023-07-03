using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;
    CanvasGroup canvasGroup;
    [SerializeField] Animator animator;
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Open(string content)
    {
        animator.SetTrigger("tFadeIn");
        canvasGroup.blocksRaycasts = true;
        text.text = content;
    }
    public void Close()
    {
        animator.SetTrigger("tFadeOff");
        canvasGroup.blocksRaycasts = false;
        text.text = "";
    }
}
