using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenObservations : MonoBehaviour
{
    QuerySnapshot results;
    [SerializeField] Button newObservationButton;
    [SerializeField] GameObject observationPrefab;

    [SerializeField] string observationRoute = "Observação";

    [SerializeField]TextMeshProUGUI patrimonioText;

    [Header("Observation Container")]
    [SerializeField] int volumeinicial = 10;
    [SerializeField] GameObject containerRef;
    List<GameObject> objectPool = new();

    [Header("Systems")]
    [SerializeField]BdOps database;
    [SerializeField]ScreenNavigator navigator;

    Coroutine delayedFitter;
    [SerializeField] float sizeFitterDelay;
    [SerializeField] ContentSizeFitter sizeFitter;
    [SerializeField] GameObject scrollarea;

     private void Awake()
    {
        PoolObjects(volumeinicial);
        
        sizeFitter= scrollarea.GetComponent<ContentSizeFitter>();

    }
    private void OnEnable() {

        InitializeList();  
       
        
    }

    private void Start() {
        newObservationButton.onClick.AddListener(
            ()=>{
                navigator.Navigate(observationRoute,1);
            }
        );
    }

    private void OnDisable() {
        sizeFitter.enabled = false;
        foreach(GameObject observationUIElement in objectPool)
        {
            observationUIElement.SetActive(false);
        } 
    }

    private void PoolObjects(int amount)
    {
        for (int i = 0; i < volumeinicial; i++)
        {
            objectPool.Add(Instantiate(observationPrefab, containerRef.transform));
        }
    }

    public void InitializeList()
    {
        results = database.observationQueryResult;
        containerRef.gameObject.SetActive(false);
        int amountFound = results.Count();
        if(amountFound<1) return;       
        // caso encontrou mais dados do que elementos instanciados instanciar mais;
        int differecence =( amountFound - objectPool.Count);
        if(differecence>0)
            PoolObjects(differecence);

        // inicializar cada elemento
        int index = 0;
        foreach(DocumentSnapshot documentSnapshot in results)
        {
            objectPool[index].SetActive(true);
            index++;
            
        }
        index = 0;
        foreach(DocumentSnapshot documentSnapshot in results)
        {
            ObservationUnityUIElement  observationUnitUIScript = objectPool[index].GetComponent<ObservationUnityUIElement>();
            observationUnitUIScript.Initialize(index+1,
            documentSnapshot, 
            database,
            ()=>{navigator.Navigate(observationRoute);}
            );
            index++;
        }
        containerRef.gameObject.SetActive(true);
        if(delayedFitter!=null) 
        StopCoroutine(delayedFitter);
        delayedFitter = StartCoroutine(delayedActivation());
    }
    IEnumerator delayedActivation()
    {
        yield return new WaitForSeconds(sizeFitterDelay);
        
        sizeFitter.enabled = true;

    }
}
