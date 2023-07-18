using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

public class BdOps : MonoBehaviour
{
    [SerializeField] Alert alert;
    [SerializeField] AuthHandler authHandler;
    FirebaseFirestore db;
    public DocumentSnapshot workingDeviceSnapShot { get; private set; }
    public QuerySnapshot observationQueryResult{ get; private set; }
    public DocumentSnapshot observationSnapshot{ get; private set; }
    public DocumentReference workingDocumentReference;
    public IEnumerable<DocumentSnapshot> cachedQueryResult{ get; private set; }

    [SerializeField] string collectionInUse="";
    [SerializeField] string deviceCollectionSuffix= "Devices";
    [SerializeField] string visitorCollection ="Tablets";

    private void Awake() {
        cachedQueryResult = Enumerable.Empty<DocumentSnapshot>();
    }
    void Start()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        string user = authHandler.GetCurrentUser();
        collectionInUse = visitorCollection;
        UpdateCollection();
    }

    private void UpdateCollection()
    {
        StartCoroutine(SetUpCollection());
    }

    private void OnEnable() {
        authHandler.onLogIn+= UpdateCollection;
        authHandler.onLogOff+= UpdateCollection;
    }
    private void OnDisable() {
        authHandler.onLogIn-= UpdateCollection;
        authHandler.onLogOff-= UpdateCollection;
    }

    IEnumerator SetUpCollection()
    {
        yield return new WaitUntil(()=> authHandler.AmIReady()); 
        string user = authHandler.GetCurrentUser();
        if(string.IsNullOrEmpty(user)) collectionInUse = visitorCollection;
        else collectionInUse = user+":"+deviceCollectionSuffix;
    }
    public async Task<DocumentReference> RegisterDevice(DeviceBaseData data)
    {
        if(data.patrimonio.Length<6|| data.IMEI.Length<15) return null;
        Dictionary<string, object> tablet = new Dictionary<string, object>
        {
            { "IMEI", data.IMEI },
            { "patrimonio", data.patrimonio },
            { "paradeiro", data.paradeiro},
            {"status",data.status },
        };
        var result  = await db.Collection(collectionInUse).AddAsync(tablet);
        workingDocumentReference = result;
        alert.Open("Tablet cadastrado!");
        return result;
    }

    public async Task<DocumentReference> RegisterObservation(ObservationData obsData, DocumentReference docRef)
    {

        CollectionReference collectionReference = docRef.Collection("observations");
        DocumentReference newDocRef = collectionReference.Document();

        Timestamp currentTime = Timestamp.FromDateTime(DateTime.Now);
        if(currentTime==null) return null;

        Dictionary<string, object> observation = new Dictionary<string, object>
        {
            {"title", obsData.title},
            {"content", obsData.content},
            {"tag", obsData.tag},
            {"timestamp", currentTime},
        };

        var result = await collectionReference.AddAsync(observation);
        alert.Open("Observação Registrada!");
        return result;
    }

    public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceObservations( DocumentReference documentReference)
    {
        Debug.Log("document reference: "+documentReference);
        CollectionReference collectionRef = documentReference.Collection("observations");
        Debug.Log("Collection reference: "+ collectionRef);
        observationQueryResult = await collectionRef.GetSnapshotAsync();

        return observationQueryResult;
        // foreach(DocumentSnapshot snapshot in result)
        // {

        // }
        // Query query = collectionRef.WhereEqualTo(keyType, key);
        // QuerySnapshot snapshot = await query.GetSnapshotAsync();
        // if(snapshot.Count>0)
        // {
        //     workingTabletSnapShot  = snapshot.Documents.FirstOrDefault();
        //     workingDocumentReference = workingTabletSnapShot.Reference;  
        // }
    }

    public async Task<DocumentReference> TryRegisterDevice(DeviceBaseData data)
    {
        //checa primeiro se já existe um dispostivo com o imei ou patrimonio fornecido
        DocumentSnapshot existingDevice = await AsyncQueryDeviceByUniqueKey(data.IMEI);
        if(existingDevice!=null && existingDevice.Exists)
        {
            alert.Open("Falha: Já existe dispositivo com o IMEI: "+data.IMEI);
            Debug.Log("Falha: Já existe dispositivo com o IMEI: "+data.IMEI);
            return null;
        } 

        existingDevice = await AsyncQueryDeviceByUniqueKey(data.patrimonio,"patrimonio");
        if(existingDevice!=null && existingDevice.Exists) 
        {
            alert.Open("Falha: Já existe dispositivo com o patrimonio: "+data.patrimonio);
            Debug.Log("Falha: Já existe dispositivo com o patrimonio: "+data.patrimonio);
            return null;
        }
        //caso já não haja dispositivo com os dados fornecidos, realizar cadastro.
       return await RegisterDevice(data);
    }

    public async Task OverwriteDevice(string docId, DeviceBaseData data)
    {
        Dictionary<string, object> device = new Dictionary<string, object>
        {
            { "IMEI", data.IMEI },
            { "patrimonio", data.patrimonio },
            { "paradeiro", data.paradeiro},
            {"status",data.status },
        };
        await db.Collection(collectionInUse).Document(docId).SetAsync(device).
            ContinueWith((task) =>
            {
                if (task.IsCompleted)
                {
                alert.Open("Dispositivo atualizado com sucesso!");
                }
                else if (task.IsFaulted)
                {
                alert.Open("Erro ao atualizar Dispositivo!");
                }
            });
    }
    public async Task<DocumentSnapshot> AsyncQueryDeviceByUniqueKey( string key, string keyType= "IMEI")
    {
        FlushCachedDeviceData();
        CollectionReference collectionRef = db.Collection(collectionInUse);
        Query query = collectionRef.WhereEqualTo(keyType, key);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        if(snapshot.Count>0)
        {
            workingDeviceSnapShot  = snapshot.Documents.FirstOrDefault();
            workingDocumentReference = workingDeviceSnapShot.Reference;  
        }
        return workingDeviceSnapShot;
    }

     public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByParameter(string paramName , int paramNumber)
    {
        
        CollectionReference collectionRef = db.Collection(collectionInUse);
        Query query = collectionRef.WhereEqualTo(paramName, paramNumber);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents;
    }
    public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByMultiParamets(List<KeyValuePair<string, int>> parameters)
    {
        
        CollectionReference collectionRef = db.Collection(collectionInUse);

        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>() ;
        foreach(KeyValuePair<string, int>parameter in parameters)
        {
            cachedQueryResult = result.Union(await AsyncQueryDeviceByParameter(parameter.Key,parameter.Value)) ;
            result = cachedQueryResult;
        }
        return result;
    }

    public async Task<IEnumerable<DocumentSnapshot>> AsyncQueryDeviceByDictionaryWithExclusions(List<KeyValuePair<string, int>>  parameters,List<KeyValuePair<string, int>>  exclusions)
    {
        CollectionReference collectionRef = db.Collection(collectionInUse);

        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>() ;

        result = await AsyncQueryDeviceByMultiParamets(parameters);
        if(exclusions!=null)
        cachedQueryResult =result.Except(await AsyncQueryDeviceByMultiParamets(exclusions));
        return result;
    }

    public async Task DeleteDevice(string id)
    {
        DocumentReference DevicesRef = db.Collection(collectionInUse).Document(id);
        await DevicesRef.DeleteAsync().ContinueWith((task) => {

            if (task.IsCompleted)
            {
                alert.Open("Dispositivo excluido com sucesso!");  
            }

            else if (task.IsFaulted) 
            { 
                alert.Open("Falha ao excluir dispositivo!");  
            }
        });
    } 

    public void UpdateWorkingDeviceSnapshot(DocumentSnapshot snap)
    {
        workingDeviceSnapShot = snap;
        workingDocumentReference= workingDeviceSnapShot.Reference; 
    }
    public void UpdateWorkingObservationSnapshot(DocumentSnapshot snap)
    {
        observationSnapshot = snap;

    }

    public void FlushCachedDeviceData()
    {
        workingDeviceSnapShot = null;
        workingDocumentReference = null;
    }

    public void Purge()
    {
        CollectionReference collectionRef = db.Collection(collectionInUse);
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
public struct DeviceBaseData
{
    public string patrimonio;
    public string IMEI;
    public int status;
    public int paradeiro;
}

public struct ObservationData
{
    public string title;
    public string content;
    public int tag;
    
}
