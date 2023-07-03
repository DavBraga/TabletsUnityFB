using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using System.Linq;
using System.Threading.Tasks;


public class AuthHandler : MonoBehaviour
{ 
    FirebaseAuth auth;
    private void Start() {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
    //     bool falhou = false;

    //    await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //     {
    //         FirebaseApp app = FirebaseApp.DefaultInstance;
    //         auth = FirebaseAuth.DefaultInstance;
    //         if(task.IsFaulted)
    //         {
    //             falhou = true;
    //         }
    //     });

    //     if(falhou) Debug.Log("falha de comunicação com o firebase");
    // }
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
            
        }
        }); 
        return sucesso;
    }
    public bool CheckAdmin()
    {   
        if(auth.CurrentUser.UserId== GlobalData.adminID) return true;
        else return false;
    }
}
