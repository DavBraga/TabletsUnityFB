using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using UnityEngine;
using TMPro;

public class MultiResultHandler : MonoBehaviour
{
    // banco de dados
    [Header("Banco de Dados")]
    [SerializeField] BdOps databaseHandler;
    //navegação
    [Header ("Navegação")]
    [SerializeField]ScreenNavigator screenNavigator;
    [SerializeField] string resultRoute= "resultado";
    [Header("ELementos de UI")]
    [SerializeField] TextMeshProUGUI numeroEncontrado;
    [SerializeField] GameObject containerDeTablets;
    [SerializeField] GameObject scrollarea;

    // prefab modelo de elmento a ser gerado;
    [SerializeField] GameObject prefabUITabletUnit;
    [SerializeField] int volumeinicial=10;

    ContentSizeFitter sizeFitter;
    [Header("Size Fitter Delay")]
    [SerializeField] float sizeFitterDelay = 0.5f;
    Coroutine delayedFitter;

    List<GameObject> objectPool = new();

    private void Awake()
    {
        PoolObjects(volumeinicial);
        sizeFitter= scrollarea.gameObject.GetComponent<ContentSizeFitter>();

    }

    private void PoolObjects(int amount)
    {
        for (int i = 0; i < volumeinicial; i++)
        {
            objectPool.Add(Instantiate(prefabUITabletUnit, containerDeTablets.transform));
        }
    }

    private void OnEnable() {

        InitializeList();  
       
        
    }
    private void OnDisable() {
        sizeFitter.enabled = false;
        foreach(GameObject tabletUIElement in objectPool)
        {
            tabletUIElement.SetActive(false);
        } 
    }

    // instanciar dinamicamente elementos da lista
    public void InitializeList()
    {
        containerDeTablets.gameObject.SetActive(false);
        int amountFound = databaseHandler.cachedQueryResult.Count();
        if(amountFound<1) return;

        numeroEncontrado.text = databaseHandler.cachedQueryResult.Count().ToString();
        
        // caso encontrou mais dados do que elementos instanciados instanciar mais;
        int differecence =( amountFound - objectPool.Count);
        if(differecence>0)
            PoolObjects(differecence);

        // inicializar cada elemento
        int index = 0;
        foreach(DocumentSnapshot documentSnapshot in databaseHandler.cachedQueryResult)
        {
            objectPool[index].SetActive(true);
            index++;
            
        }
        index = 0;
        foreach(DocumentSnapshot documentSnapshot in databaseHandler.cachedQueryResult)
        {
            TabletUnitUIElement tabletUnitUIScript = objectPool[index].GetComponent<TabletUnitUIElement>();
            tabletUnitUIScript.Initialize(index+1,
            documentSnapshot, 
            databaseHandler,
            ()=>{screenNavigator.Navigate(resultRoute,1);}
            );
            index++;
        }
        containerDeTablets.gameObject.SetActive(true);
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
