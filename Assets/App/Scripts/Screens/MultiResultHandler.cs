using System.Collections;
using System.Linq;
using System.Collections.Generic;
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

    // prefab modelo de elmento a ser gerado;
    [SerializeField] GameObject prefabUITabletUnit;
    [SerializeField] int volumeinicial=10;

    List<GameObject> objectPool = new();

    private void Awake()
    {
        PoolObjects(volumeinicial);

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
        foreach(GameObject tabletUIElement in objectPool)
        {
            tabletUIElement.SetActive(false);
        } 
    }

    // instanciar dinamicamente elementos da lista
    public void InitializeList()
    {
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
            ()=>{screenNavigator.Navigate(resultRoute);}
            );
            index++;
        }
    }
}
