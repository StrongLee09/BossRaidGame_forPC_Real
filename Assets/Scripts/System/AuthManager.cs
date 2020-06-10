using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

public class AuthManager : MonoBehaviour
{

    #region Public Field
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;


    #endregion

    #region Private Methods



    #endregion

    #region MonoBehaviour Callback
    void Start()
    {

        signInButton.interactable = false;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
        {
            var result = task.Result;

            if (result != DependencyStatus.Available)
            {
                Debug.LogError(message: result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                Debug.Log("접속 성공");
                IsFirebaseReady = true;
               UserDataManager.Instance.firebaseApp= FirebaseApp.DefaultInstance;
                UserDataManager.Instance.firebaseAuth = FirebaseAuth.DefaultInstance;
                signInButton.interactable = IsFirebaseReady;
            }

            signInButton.interactable = IsFirebaseReady;
            Debug.Log(IsFirebaseReady);
        });
    }

    private void Update()
    {
        if (UserDataManager.Instance.isloading == true)
        {
            Debug.Log("로드 로비씬");
            SceneManager.LoadScene("Lobby");
            UserDataManager.Instance.isloading = false;
        }
    }

    #endregion

    #region Public Methods
    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || UserDataManager.Instance.User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        UserDataManager.Instance.firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread((task) =>
        {

            Debug.Log(message: $"Sign in status : {task.Status}");

            IsSignInOnProgress = false;
            signInButton.interactable = true;
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError(message: "Sign-in canceled");
            }
            else
            {
                UserDataManager.Instance.User = task.Result;
                UserDataManager.Instance.SetUserId();
                UserDataManager.Instance.SearchUser();
                Debug.Log(UserDataManager.Instance.User.Email);
            }
        });
    }
    #endregion

    #region Private Methods
    #endregion


}
