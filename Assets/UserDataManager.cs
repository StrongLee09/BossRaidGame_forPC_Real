using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class UserDataManager : MonoBehaviour
{
    #region Singleton
    private static UserDataManager instance;
    public static UserDataManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UserDataManager>();
            return instance;
        }
    }

    #endregion

    public UserInfo2 userInfo;
    public bool isfind;
    public bool isUserfirst;
    public bool isloading;
    public FirebaseApp firebaseApp;
    public FirebaseAuth firebaseAuth;
    public FirebaseUser User;
    public string UserID;
    public string UserNickName;

    private void Awake()
    {
        //씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 
        if (Instance != this)
        {
            //파괴
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Debug.Log("초기화 시작");
        //userInfo.Initialize();
        isfind = false;
        isUserfirst = false;
        isloading = false;
    }

    public void SetUserId()
    {
        UserID= User.Email.Substring(0,User.Email.IndexOf("@"));
    }

    public void SearchUser()
    {
        userInfo.SearchUser(UserID);
    }
    public void SearchUserNickName(string usernickname)
    {
        userInfo.SearchUserNickName(usernickname);
    }

    public void AddUser()
    {
        userInfo.AddUser(UserID);
        
    }
    public void GetUser()
    {
        userInfo.GetUser(UserID);
    }

    public void SetUserNickName(string nickname)
    {
        userInfo.SetUserNickName(nickname);
    }
    //종료시 초기화
    private void OnApplicationQuit()
    {
        this.userInfo.user.usernickname = "";
        this.userInfo.user.usergold = 0;
        this.userInfo.user = new User1();
        this.userInfo = new UserInfo2();
    }

}
