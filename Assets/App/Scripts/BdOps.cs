using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Device;
using UnityEngine.UIElements;
using System.Linq;

public class BdOps : MonoBehaviour
{
    FirebaseFirestore db;
    public DocumentSnapshot tabletData { get; private set; }
    List<DocumentSnapshot> documentSnapshots = new List<DocumentSnapshot>();
    public bool querying {get;private set;}
    // Start is called before the first frame update
    void Start()
    {
        querying = false;
        db = FirebaseFirestore.DefaultInstance;
    }
    public async void CatalogarTablet(TabletbaseData data)
    {
        Dictionary<string, object> tablet = new Dictionary<string, object>
        {
            { "IMEI", data.IMEI },
            { "patrimonio", data.patrimonio },
            { "paradeiro", data.paradeiro},
            {"status",data.status }
        };
        DocumentReference docRef = await db.Collection("Tablets").AddAsync(tablet);
    }
    public async void SobreEscreverTablet(string docId, TabletbaseData data)
    {
        Dictionary<string, object> tablet = new Dictionary<string, object>
        {
            { "IMEI", data.IMEI },
            { "patrimonio", data.patrimonio },
            { "paradeiro", data.paradeiro},
            {"status",data.status }
        };
       await db.Collection("Tablets").Document(docId).SetAsync(tablet).
             ContinueWith((task) =>
             {
                 if (task.IsCompleted)
                 {
                     Debug.Log("Documento sobrescrito com sucesso!");
                 }
                 else if (task.IsFaulted)
                 {
                     Debug.LogError("Erro ao sobrescrever o documento: " + task.Exception);
                 }
             });
    }
    public async void ConsultarTabletPorParam(string property, int queryParam)
    {
        CollectionReference tabletsRef = db.Collection("Tablets");
        Query query = tabletsRef.WhereEqualTo(property, queryParam);

        querying = true;
        await query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int count = querySnapshotTask.Result.Documents.Count();
            if (count > 0)
            {
                List<DocumentSnapshot> documentSnapshots = querySnapshotTask.Result.Documents.ToList();
            }
            if (querySnapshotTask.IsCompleted) querying = false;
        });
    }

    public async void ConsultarTabletPorDuploParam(string property, string property2, int queryParam, int queryParam2)
    {
        CollectionReference tabletsRef = db.Collection("Tablets");
        Query query = tabletsRef.WhereEqualTo(property, queryParam)
                                .WhereEqualTo(property2, queryParam2);

        querying = true;
        await query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int count = querySnapshotTask.Result.Documents.Count();
            if (count > 0)
            {
                List<DocumentSnapshot> documentSnapshots = querySnapshotTask.Result.Documents.ToList();
                
            }
            if(querySnapshotTask.IsCompleted) querying = false;
        });
    }

    public async void ConsultarTabletPorChave( string key, string keyType= "IMEI")
    {

        CollectionReference collectionRef = db.Collection("Tablets");
        Query query = collectionRef.WhereEqualTo(keyType, key);
        querying = true;
        await query.GetSnapshotAsync().ContinueWith((task) =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;
                Debug.Log(snapshot.Documents.Count());
                if (snapshot.Documents.Count() > 0)
                {
                    DocumentSnapshot documentSnapshot = snapshot.FirstOrDefault();
                    if (documentSnapshot.Exists)
                    {
                        tabletData = documentSnapshot;
                    }
                }
                querying = false;
            }
        });


    }

    public async void ExcluirTablet(string id)
    {
        DocumentReference tabletsRef = db.Collection("Tablets").Document(id);
        await tabletsRef.DeleteAsync().ContinueWith((task) => {

            if (task.IsCompleted)
            {
                Debug.Log("Document deleted successfully!");
            }

            else if (task.IsFaulted) 
            { 
                Debug.LogError("Error deleting document: " + task.Exception);   
            }
        });
    } 

    public void ClearTabletData()
    {
        tabletData = null;
    }
}


public struct TabletbaseData
{
    public string patrimonio;
    public string IMEI;
    public int status;
    public int paradeiro;
}
