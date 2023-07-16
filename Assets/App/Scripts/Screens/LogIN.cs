using UnityEngine;
using TMPro;


public class LogIN : MonoBehaviour
{
    [SerializeField] AuthHandler authHandler;
    [SerializeField] TMP_InputField email;
    [SerializeField]TMP_InputField password;
    [SerializeField] Alert alert;
    [SerializeField] LoadingScreen load;
    [SerializeField] ScreenNavigator screenNavigator;
    public async void ButtonLogIn()
    {
        load.OpenLoadScreen();
       if(! await authHandler.Logon(email.text, password.text))
       {
            load.CloseLoadScreen();
            alert.Open("Login inválido!");
       }
       else
       {
        screenNavigator.Navigate("home");
       } 
    }
    public async void ButtonRegister()
    {
       if(! await authHandler.RegisterUser(email.text,password.text))
       {
            alert.Open("Falha ao tentar registrar novo usuário.");
       }
       else alert.Open("Novo usuário registrado com sucesso!");
    }
}
