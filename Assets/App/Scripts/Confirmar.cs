using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Confirmar : MonoBehaviour
{
    public UnityAction confirm;
    public void CallConfirmar(UnityAction action)
    {
        confirm = action;
    }

    public void OnConfirmed()
    {
        confirm?.Invoke();
        gameObject.SetActive(false);
    }
}
