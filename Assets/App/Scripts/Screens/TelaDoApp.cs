using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaDoApp : MonoBehaviour
{
    CanvasGroup group;
    [SerializeField] string nomeDaTela ="";
    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    public void TransitIn() 
    { 
        group.alpha= 1.0f;
    }
    public void TransitOut()
    {
        group.alpha = 0.0f;
        gameObject.SetActive(false);
    }

    public string GetScreenName()
    {
        if (nomeDaTela.Length < 1) return "" ;

        return nomeDaTela;
    }
}
