using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppChromeSettings : MonoBehaviour
{
        
    // Start is called before the first frame update
    void Start()
    {
        ApplicationChrome.statusBarState = ApplicationChrome.States.Visible;
       // ApplicationChrome.navigationBarState = ApplicationChrome.States.v;
        ApplicationChrome.dimmed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
