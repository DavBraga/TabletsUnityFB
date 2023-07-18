using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

public class AuthHandler : MonoBehaviour
{ 
    public bool isReady = false;
    [SerializeField] BdOps databaseHandler;
    FirebaseAuth auth;
    public UnityAction onLogIn, onLogOff;
    private void Start() {
        isReady =false;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        isReady = true;
    }
    public async Task<bool> RegisterUser(string email, string password)
    {
        bool success = false;
       await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task=>{
        if(task.IsCompletedSuccessfully)
        {
                Debug.Log("Cadastro realizado com sucesso");
                success = true;
        }else success = false;
       });
       
        return success;
       
    }

    public async Task<bool> Logon(string email,string password)
    {
        bool sucesso = false;
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
            Debug.LogError("log in foi cancelado");
            sucesso = false;
            }
        if (task.IsFaulted) {

            //alert.Open("falha ao logar " + task.Exception);
            sucesso = false;
        }
        if(task.IsCompletedSuccessfully)
        {
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
            sucesso = true;
            onLogIn?.Invoke();
            
        }
        }); 
        return sucesso;
    }

    public void LogOff()
    {
        auth.SignOut();
    }
    public bool AmIReady()
    {
        return isReady;
    }

    public string GetCurrentUser()
    {
        if(isReady&&auth.CurrentUser!=null)
       return auth.CurrentUser.Email;
       else return null;
    }
}
