using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ScreenNavigator : MonoBehaviour
{
    [SerializeField] private List<TelaDoApp> telasDoApp = new List<TelaDoApp>();
    [SerializeField] string telaInicial = "home";

    private Dictionary<string, TelaDoApp> rotas= new Dictionary<string, TelaDoApp>();
    private List<string> ultimaTelaVisitada = new List<string>();
    private string telaAtual="";
    public UnityAction onScreenTransit;

    private void Awake()
    {
        PopularRotas();
    }
    // Start is called before the first frame update
    void Start()
    {
        Navigate(telaInicial);
    }
    public void PopularRotas()
    {
        // se nenhuma rota foi fornecidam ignorar;
        if (telasDoApp.Count < 1) return;

        // limpar dicionário antes de usar;
        rotas.Clear();
        int index = 0;
        foreach(TelaDoApp rota in telasDoApp)
        {
            // caso o nome da tela não foi definido, usar o index;
            if(rota.GetScreenName()=="") rotas.Add(index.ToString(), rota);
            rotas.Add(rota.GetScreenName(), rota);
            index++;
        }
    }
    public void Navigate(string telaASerVisitada = "home")
    {
        TelaDoApp screen;
        // sair da tela anteriro
        if(telaAtual!="")
        {
            rotas.TryGetValue(telaAtual, out screen);
            ultimaTelaVisitada.Add(screen.GetScreenName());
            screen.TransitOut();
        }
        
        //entrar na nova tela
        rotas.TryGetValue(telaASerVisitada, out screen);
        screen.gameObject.SetActive(true);
        screen.TransitIn();
        this.telaAtual = screen.GetScreenName();
        onScreenTransit?.Invoke();

    }

    public void Navigate(bool returning, string telaASerVisitada = "home" )
    {
        TelaDoApp screen;
        // sair da tela anteriro
        if (telaAtual != "")
        {
            rotas.TryGetValue(telaAtual, out screen);
            if(!returning)
            ultimaTelaVisitada.Add(screen.GetScreenName());
            screen.TransitOut();
        }

        //entrar na nova tela
        rotas.TryGetValue(telaASerVisitada, out screen);
        screen.gameObject.SetActive(true);
        screen.TransitIn();
        this.telaAtual = screen.GetScreenName();
        onScreenTransit?.Invoke();
    }

    public void NavigateBack()
    {  
        if(ultimaTelaVisitada.Count>0)
        {
            Navigate( true, ultimaTelaVisitada.Last());
            ultimaTelaVisitada.RemoveAt(ultimaTelaVisitada.Count-1);
        }
        /* //todo
        if (ultimaTelaVisitada == "") return;*/
    }

    public string GetCurrentScreen()
    {
        return telaAtual;
    }
}
