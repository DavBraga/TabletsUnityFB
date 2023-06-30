using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class QueryHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    [SerializeField] ScreenNavigator screenNavigator;
    [SerializeField] string resultRoute = "resultado";
    [SerializeField] Alert alert;

    public UnityAction onTabletFound;
    public UnityAction onTabletNotFound;
    bool isIMEIValid;
    bool isPatrimonioValid;
    public bool isWaitingQuery = false;

    [SerializeField] BdOps databaseHandler;

    public void QueryData()
    {
        if (databaseHandler.querying) { Debug.Log("DB is busy..."); return; }
        
        isIMEIValid = !string.IsNullOrEmpty(IMEI.text);
        isPatrimonioValid = !string.IsNullOrEmpty(patrimonio.text);
        

        StartCoroutine(WaitForQueries());
    }

    IEnumerator WaitForQueries()
    {
        if (isWaitingQuery) {  yield break; }
        databaseHandler.ClearTabletData();
        if (isIMEIValid)
        {

            databaseHandler.ConsultarTabletPorChave(IMEI.text);
        }
            

        isWaitingQuery = true;
        yield return new WaitUntil(() => databaseHandler.querying == false);
        //se não encontrou pelo imei, tentar pelo patrimonio
        if (databaseHandler.tabletData== null)
        {
            if (isPatrimonioValid)
            {
                databaseHandler.ConsultarTabletPorChave(patrimonio.text, "patrimonio");
            }
               
        }
        yield return new WaitUntil(() => databaseHandler.querying == false);

        // alerta se não encontrar, abre próxima pagina se encontrar.
        if (databaseHandler.tabletData == null)
        {
            alert.gameObject.SetActive(true);
            alert.Call("Tablet não encontrado!");
        }
        else
        {
            OpenResultScreen();
            onTabletFound?.Invoke();
        }

        isWaitingQuery = false;
    }
    public void OpenResultScreen()
    {
         screenNavigator.Navigate(resultRoute);
       // DebugResult();
    }

    public void DebugResult()
    {
        Dictionary<string, object> tablet = databaseHandler.tabletData.ToDictionary();
        tablet.TryGetValue("patrimonio", out object obj);
        Debug.Log("patrimonio: " + obj);
    }
}
