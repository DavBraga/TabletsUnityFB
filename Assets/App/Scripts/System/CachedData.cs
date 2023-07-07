using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachedData : MonoBehaviour
{
    string barcodeData = "";

    public void CacheBarCodeData(string data)
    {
        if(string.IsNullOrWhiteSpace(data)||string.IsNullOrEmpty(data))barcodeData = "dados não lidos";
        barcodeData = data;
    }
    public string GetCachedBarCodeData()
    {
        return barcodeData;
    }
}
