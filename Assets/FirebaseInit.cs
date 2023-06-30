using Firebase;
using Firebase.Analytics;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseInit : MonoBehaviour
{
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    public Text text;

    void Start()
    {
        text = GetComponent<Text>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
           /// FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
           // Debug.Log("im here");
            //var auth = FirebaseAuth.DefaultInstance;
            //auth.CreateUserWithEmailAndPasswordAsync("costdavi@gmail.com", "A123456789b");
        });

        DocumentReference docRef = db.Collection("Tablets").Document("LA");

        Dictionary<string, object> city = new Dictionary<string, object>
{
        { "Name", "Los Angeles" },
        { "State", "CA" },
        { "Country", "USA" }
};
        docRef.SetAsync(city).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the LA document in the cities collection.");
        });
    }
}

