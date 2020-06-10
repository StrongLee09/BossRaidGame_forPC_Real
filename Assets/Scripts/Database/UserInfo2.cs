using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Photon.Pun.Demo.Cockpit;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "New User Profile", menuName = "DataBase System/Users/Profile")]
[System.Serializable]
public class UserInfo2 : ScriptableObject
{
    //6.8 유저 데이터 매니저 유저인포 수정하기

    
    public User1 user =new User1();


    #region 보수가 필요한 쓰레기 기능입니다.

    //DatabaseReference reference;
    //DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    //[ContextMenu("Init Fireabase")]
    //public void Initialize()
    //{
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    //    {
    //        dependencyStatus = task.Result;
    //        if(dependencyStatus == DependencyStatus.Available)
    //        {
    //            InitializeFirebase();
    //        }
    //        else
    //        {
    //            Debug.LogError("Could not resolve all Firebase dependencies:" + dependencyStatus);
    //        }
    //    });
    //}
    //public void InitializeFirebase()
    //{
    //    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitybossraidgame.firebaseio.com/");
    //    this.reference = FirebaseDatabase.DefaultInstance.RootReference;
    //    Debug.Log(reference);
    //}
    #endregion


    public void AddUser(string userId)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitybossraidgame.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string saveData = JsonUtility.ToJson(this.user, true);
        Debug.Log(this.user.usergold);
        Debug.Log(this.user.usernickname);
        reference.Child("users").Child(userId).SetRawJsonValueAsync(saveData);
        UserDataManager.Instance.isloading = true;
    }

    public void SetUserNickName(string usernickname)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitybossraidgame.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        this.user.usernickname = usernickname;
        Debug.Log("니익네임" + user.usernickname);
        UserDataManager.Instance.UserNickName = usernickname;
        reference.Child("users").Child(UserDataManager.Instance.UserID)
            .Child("usernickname").SetValueAsync(usernickname);
    }

    //서치 유저...
    public void SearchUser(string userId)
    {
        FirebaseDatabase.DefaultInstance.RootReference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChild("users"))
                {
                    FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task2 =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.Log("Failed");
                        }
                        else if (task.IsCompleted)
                        {
                            Debug.Log("서치들어감");
                            DataSnapshot snapshot2 = task.Result;
                            Debug.Log(snapshot2.GetRawJsonValue().Contains(userId));
                            Debug.Log(snapshot2.GetRawJsonValue());
                            Debug.Log(snapshot2.Value.ToString());
                            if (snapshot2.GetRawJsonValue().Contains(userId))
                            {
                                UserDataManager.Instance.isUserfirst = false;
                                Debug.Log("User find " + userId);
                                UserDataManager.Instance.GetUser();
                                return;
                            }
                            else
                            {
                                UserDataManager.Instance.isUserfirst = true;
                                Debug.Log("Can't access user");
                                Debug.Log("새로운 유저 등록 !!");
                                UserDataManager.Instance.AddUser();
                                return;
                            }
                        }
                    });
                }
                else
                {
                    UserDataManager.Instance.isUserfirst = true;
                    Debug.Log("Can't access user");
                    Debug.Log("새로운 유저 등록 !!");
                    UserDataManager.Instance.AddUser();
                }
            }
        });
        Debug.Log("써치끝");
    }

    //서치 유저닉네임
    public void SearchUserNickName(string userNickname)
    {
        FirebaseDatabase.DefaultInstance.RootReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChildren)
                {
                    foreach(var childSnapshot in snapshot.Children)
                    {
                        if (childSnapshot.GetRawJsonValue().Contains(userNickname))
                        {
                            Debug.Log("존재하는 닉네임  " + userNickname);
                            UserDataManager.Instance.isloading = true;
                            UserDataManager.Instance.isfind = true;
                        }
                        else
                        {
                            Debug.Log("존재하지 않는 닉네임");
                            Debug.Log("닉네임 등록!!");
                            UserDataManager.Instance.SetUserNickName(userNickname);
                            UserDataManager.Instance.isloading = true;
                            UserDataManager.Instance.isfind = false;
                        }
                    }
                }
            }
        });
    }

    //겟 유저
    public void GetUser(string userId)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string saveData = snapshot.GetRawJsonValue();
                JsonUtility.FromJsonOverwrite(saveData, this.user);
                Debug.Log(this.user.usergold);
                Debug.Log(this.user.usernickname);
            }
        });
        UserDataManager.Instance.isloading = true;
    }

}
[System.Serializable]
public class User1
{
    // 유저 정보에 저장할 데이터 선언
    public string usernickname;
    public int usergold;
    //public Item item;
    public User1()
    {
        usernickname = "null";
        usergold = 0;
    }
}

//public class Item
//{

//}
