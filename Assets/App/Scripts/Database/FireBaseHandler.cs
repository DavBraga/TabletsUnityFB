using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;

public class FireBaseHandler : MonoBehaviour
{
    FirebaseAuth auth;
    [SerializeField]GameObject[] firebaseHandlers;
    bool falhou;
    void Start()
    {
        StartFireBase();
    }

    async void StartFireBase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            if(task.IsFaulted)
            {
                falhou = true;
            }
        });
        if(!falhou) FireupFirebaseHandlers();
    }

    public void FireupFirebaseHandlers()
    {
        foreach(GameObject handler in firebaseHandlers)
        {
            handler.gameObject.SetActive(true);
        }
    }
}
