using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Confirmar : MonoBehaviour
{
    public UnityAction onConfirm;
    public UnityAction onNegate;
    public void CallConfirmar(UnityAction action)
    {
        onConfirm = action;
    }

    public void OnConfirmed()
    {
        onConfirm?.Invoke();
        gameObject.SetActive(false);
        onConfirm = null;
    }
    public void OnNegate()
    {
        onNegate?.Invoke();
        gameObject.SetActive(false);
        onNegate = null;
    }

    public void Flush()
    {
        onConfirm = null;
        onNegate = null;
    }
}
