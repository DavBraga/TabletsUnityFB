using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollection : MonoBehaviour
{
    FirebaseFirestore db;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    private void Start()
    {
        if (db != null) { Debug.Log("Got db"); }
    }

    public void AddCollectionFunc()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            /// FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            // Debug.Log("im here");
            //var auth = FirebaseAuth.DefaultInstance;
            //auth.CreateUserWithEmailAndPasswordAsync("costdavi@gmail.com", "A123456789b");
        });
        DocumentReference docRef = db.Collection("cities").Document("LA");

        Dictionary<string, object> city = new Dictionary<string, object>
        {
        { "Name", "Los Angeles" },
        { "State", "CA" },
        { "Country", "USA" }
        };

        docRef.SetAsync(city).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Added data to the LA document in the cities collection.");
        });
    }
    public void Read()
    {
        DocumentReference docRef = db.Collection("cities").Document("LA");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> city = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }
}
