using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Device;
using UnityEngine.UIElements;
using System.Linq;
using System.Threading.Tasks;

public class BdOps : MonoBehaviour
{
    [SerializeField] Alert alert;
    FirebaseFirestore db;
    public DocumentSnapshot workingTabletSnapShot { get; private set; }
    public IEnumerable<DocumentSnapshot> cachedQueryResult{ get; private set; }

    private void Awake() {
        cachedQueryResult = Enumerable.Empty<DocumentSnapshot>();
    }
    void Start()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
       
    }
    public async Task<DocumentReference> RegisterDevice(TabletbaseData data)
    {
        if(data.patrimonio.Length<6|| data.IMEI.Length<15) return null;
        Dictionary<string, object> tablet = new Dictionary<string, object>
        {
            { "IMEI", data.IMEI },
            { "patrimonio", data.patrimonio },
            { "paradeiro", data.paradeiro},
            {"status",data.status }
        };
        var result  = await db.Collection("Tablets").AddAsync(tablet);
        alert.Open("Tablet cadastrado!");
        return result;
    }

    public async Task<DocumentReference> TryRegisterDevice(TabletbaseData data)
    {
        //checa primeiro se já existe um tablet com o imei ou patrimonio fornecido
        DocumentSnapshot existingTablet = await AsyncQueryDeviceByUniqueKey(data.IMEI);
        if(existingTablet!=null && existingTablet.Exists)
        {
            alert.Open("Falha: Já existe tablet com o IMEI: "+data.IMEI);
            Debug.Log("Falha: Já existe tablet com o IMEI: "+data.IMEI);
            return null;
        } 

        existingTablet = await AsyncQueryDeviceByUniqueKey(data.patrimonio,"patrimonio");
        if(existingTablet!=null && existingTablet.Exists) 
        {
            alert.Open("Falha: Já existe tablet com o patrimonio: "+data.patrimonio);
            Debug.Log("Falha: Já existe tablet com o patrimonio: "+data.patrimonio);
            return null;
        }
        //caso já não haja tablet com os dados fornecidos, realizar cadastro.
       return await RegisterDevice(data);
    }

    public async Task OverwriteDevice(string docId, TabletbaseData data)
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
                alert.Open("Tablet atualizado com sucesso!");
                }
                else if (task.IsFaulted)
                {
                alert.Open("Erro ao atualizar tablet!");
                }
            });
    }
    public async Task<DocumentSnapshot> AsyncQueryDeviceByUniqueKey( string key, string keyType= "IMEI")
    {
        FlushCachedTabletData();
        CollectionReference collectionRef = db.Collection("Tablets");
        Query query = collectionRef.WhereEqualTo(keyType, key);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        if(snapshot.Count>0)
        {
            workingTabletSnapShot  = snapshot.Documents.FirstOrDefault();  
        }
        return workingTabletSnapShot;
    }

     public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByParameter(string paramName , int paramNumber)
    {
        
        CollectionReference collectionRef = db.Collection("Tablets");
        Query query = collectionRef.WhereEqualTo(paramName, paramNumber);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        if(snapshot.Count<1)
        {
            alert.Open("Nenhum tablet encontrado!");
        }
        return snapshot.Documents;
    }
    public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByDictionary(Dictionary<string,int> parameters)
    {
        
        CollectionReference collectionRef = db.Collection("Tablets");

        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>() ;
        foreach(KeyValuePair<string, int>parameter in parameters)
        {
            Debug.Log("by key:"+ parameter.Key);
          cachedQueryResult =  result = result.Union(await AsyncQueryDeviceByParameter(parameter.Key,parameter.Value)) ;
        }
        return result;
    }

    public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByDictionaryWithExclusions(Dictionary<string,int> parameters,Dictionary<string,int> exclusions)
    {
        CollectionReference collectionRef = db.Collection("Tablets");

        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>() ;

        result = await AsyncQueryDeviceByDictionary(parameters);
        if(exclusions!=null)
        cachedQueryResult =result.Except(await AsyncQueryDeviceByDictionary(exclusions));
        return result;
    }

    public async Task DeleteDevice(string id)
    {
        DocumentReference tabletsRef = db.Collection("Tablets").Document(id);
        await tabletsRef.DeleteAsync().ContinueWith((task) => {

            if (task.IsCompleted)
            {
                alert.Open("Tablet excluido com sucesso!");  
            }

            else if (task.IsFaulted) 
            { 
                alert.Open("Falha ao excluir tablet!");  
            }
        });
    } 

    public void UpdateWorkingTabletSnapshot(DocumentSnapshot snap)
    {
        workingTabletSnapShot = snap;
    }

    public void FlushCachedTabletData()
    {
        workingTabletSnapShot = null;
    }

    public void Purge()
    {
        CollectionReference collectionRef = db.Collection("Tablets");
        collectionRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Erro ao obter documentos: " + task.Exception);
                return;
            }

            QuerySnapshot snapshot = task.Result;

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                document.Reference.DeleteAsync();
            }
        });
    }
}
public struct TabletbaseData
{
    public string patrimonio;
    public string IMEI;
    public int status;
    public int paradeiro;
}
