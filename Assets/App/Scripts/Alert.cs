using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;
    public void Call(string content)
    {
        text.text = content;
    }
    public void Close()
    {
        text.text = "";
    }
}
