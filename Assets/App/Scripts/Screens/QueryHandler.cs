using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DavsInputValidation;
using UnityEngine.UI;
public class QueryHandler : MonoBehaviour
{
    [Header("UI tools")]
    [SerializeField] Alert alert;
    [SerializeField] LoadingScreen loadingScreen;

    [Header("Navegação")]
    [SerializeField] ScreenNavigator screenNavigator;
    [SerializeField] string resultRoute = "resultado";
    [SerializeField] string resultListRoute = "resultadolista";
    [SerializeField] string scanRoute = "scanner";

    [Header("Busca por Chave Única")]
    [SerializeField] TMP_InputField patrimonio;
    [SerializeField] TMP_InputField IMEI;
    

    [Header("Paradeiro")]
    // paradeiro inputs
    [SerializeField]Toggle comResponsavel;
    [SerializeField]Toggle naEscola;
    [SerializeField]Toggle naSME;
    [SerializeField]Toggle Desaparecido;
    // status inputs
    [Header("Status")]
    [SerializeField] Toggle funcionando;[SerializeField]Toggle quebrado;
    [SerializeField]Toggle  emManutencao;
    [SerializeField]Toggle baixa; 
    public UnityAction onTabletFound;
    public UnityAction onTabletNotFound;
    bool isIMEIValid;
    bool isPatrimonioValid;
    public bool isWaitingQuery = false;

    // scanner
    bool readFromScanner= false;
    TMP_InputField targetInpuField;
    [Header("Sistemas")]
    [SerializeField] BdOps databaseHandler;
    [SerializeField] CachedData cachedData;

    private void OnEnable() {
        if(!readFromScanner) return;

        targetInpuField.text = cachedData.GetCachedBarCodeData();
        Validation.ValidateField(IMEI.textComponent,16,Color.red,Color.black);
        Validation.ValidateField(patrimonio.textComponent,7,Color.red,Color.black);
        targetInpuField.Select();
        targetInpuField = null;
        readFromScanner = false;
    }
    private void Start() {

        IMEI.onDeselect.AddListener(
            (string a)=>{
                Validation.ValidateField(IMEI.textComponent,16,Color.red,Color.black);
            });

        IMEI.onSelect.AddListener(
            (string a)=>{
                IMEI.textComponent.color = Color.black;
            });

        patrimonio.onDeselect.AddListener(
            (string a)=>{
                Validation.ValidateField(patrimonio.textComponent,7,Color.red,Color.black);
            });
            
        patrimonio.onSelect.AddListener(
            (string a)=>{
                patrimonio.textComponent.color = Color.black;
            });
    }


    public async void  AsyncQueryData()
    {
        isIMEIValid = !string.IsNullOrEmpty(IMEI.text)&& IMEI.text.Length==15;
        isPatrimonioValid = !string.IsNullOrEmpty(patrimonio.text)&& patrimonio.text.Length==6;
        if(!isIMEIValid && !isPatrimonioValid)
        {
            alert.Open("Preencha corretamente ao menos um dos campos.");
            return;   
        }

        DocumentSnapshot queryResult = null;
        loadingScreen.OpenLoadScreen();
        if(isIMEIValid)
        {
            queryResult = await databaseHandler.AsyncQueryDeviceByUniqueKey(IMEI.text);
        }
        else
        {
            queryResult = await databaseHandler.AsyncQueryDeviceByUniqueKey(patrimonio.text, "patrimonio");
        }

        loadingScreen.CloseLoadScreen();
        if(queryResult!=null && queryResult.Exists)
        {
            ResetFields();
            OpenResultScreen();
            onTabletFound?.Invoke();
            return;
        }
        alert.Open("Tablet não encontrado!");         
    }

    public async void AsyncAdvancedQueryData()
    {
        // criar dicionário com parametros de pesquisa 
        List<KeyValuePair<string, int>>  parameters = new();
        // parametros referentes ao paradeiro
        if(comResponsavel.isOn)
        parameters.Add( new KeyValuePair< string, int >("paradeiro",0 ) ); 
        if(naEscola.isOn)
        parameters.Add( new KeyValuePair< string, int >("paradeiro",1 ) ); 
        if(naSME.isOn)
        parameters.Add( new KeyValuePair< string, int >("paradeiro",2 ) ); 
        if(Desaparecido.isOn)
        parameters.Add( new KeyValuePair< string, int >("paradeiro",3 ) ); 
        // parametros referentes ao status
        if(funcionando.isOn)
        parameters.Add( new KeyValuePair< string, int >("status",0 ) ); 
        if(quebrado.isOn)
        parameters.Add( new KeyValuePair< string, int >("status",1 ) ); 
        if(emManutencao.isOn)
        parameters.Add( new KeyValuePair< string, int >("status",2 ) ); 
        if(baixa.isOn)
        parameters.Add( new KeyValuePair< string, int >("status",3 ) ); 

        //realiza busca
        loadingScreen.OpenLoadScreen();
        IEnumerable<DocumentSnapshot> result = Enumerable.Empty<DocumentSnapshot>();
        result = await databaseHandler.AsyncQueryDeviceByMultiParamets(parameters);
        loadingScreen.CloseLoadScreen();
        if(result.Count()<1)
        {
            alert.Open("Nenhum tablet encontrado com os parametros fornecidos.");
            return;
        }
        else
        {
            ResetFields();
            OpenMultiResultsScreen();
        }
    
    }

    public void OpenMultiResultsScreen()
    {
        screenNavigator.Navigate(resultListRoute);
    }
    public void OpenResultScreen()
    {
         screenNavigator.Navigate(resultRoute,1);
    }

     public void ResetFields()
    {
        IMEI.text = null;
        patrimonio.text = null;
    }

    public void ButtonScanPatrimonio()
    {
        readFromScanner = true;
        targetInpuField = patrimonio;
        screenNavigator.Navigate(scanRoute);
        
    }
    public void ButtonScanIMEI()
    {
        readFromScanner = true;
        targetInpuField = IMEI;
        screenNavigator.Navigate(scanRoute);
    }
}
