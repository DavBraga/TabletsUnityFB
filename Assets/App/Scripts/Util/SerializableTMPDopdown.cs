using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SerializableTMPDropdown : MonoBehaviour
{
    public TMP_Dropdown Dropdown
    {
        get { return GetComponent<TMP_Dropdown>();}
    }
}

