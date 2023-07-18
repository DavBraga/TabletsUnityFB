using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerScreen : MonoBehaviour
{
   [SerializeField] ScreenNavigator navigator;
   [SerializeField] CachedData cachedData;
    [SerializeField] BarCodeScanner scanner;

    public void ButtonCapture()
    {
        cachedData.CacheBarCodeData(scanner.resultData);
        navigator.NavigateBack(1);
    }
}
