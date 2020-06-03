using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

[CreateAssetMenu(fileName = "New User Profile", menuName = "DataBase System/Users/Profile")]
public class UserInfo : ScriptableObject
{
    public List<PlayerContainer> Player = new List<PlayerContainer>();

    public void AddUser(string _Email,string _Nickname)
    {
        if(Player.Count != 0)
        {
            for(int i =0; i<Player.Count; i++)
            {
                if(Player[i].userProfiles.UserEmail.Equals(_Email))
                {
                    Debug.Log("중복 아이디...");
                    return;
                    
                }
                else
                {
                    for(int j=0; j<Player.Count; j++)
                    {
                        if (Player[j].userProfiles.UserNickname == "Unknown")
                        {
                            Player.Add(new PlayerContainer(_Email, _Nickname));
                            return;
                        }
                        if (Player[j].userProfiles.UserNickname.Equals(_Nickname))
                        {
                            Debug.Log("중복 닉네임. . . ");
                        }
                        else
                        {
                            Player.Add(new PlayerContainer(_Email,_Nickname));
                            return;
                        }
                    }
                    
                }
            }
        }
        else
        {
            Player.Add(new PlayerContainer(_Email, _Nickname));
        }
        
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //version 3
        Debug.Log("서버에 저장중 !");
        
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitybossraidgame.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string saveData = JsonUtility.ToJson(this, true);
        reference.Child("Players").SetRawJsonValueAsync(saveData);
        Debug.Log("서버에 저장하기 성공 !");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        //version 3 
        FirebaseDatabase.DefaultInstance.GetReferenceFromUrl("https://unitybossraidgame.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("로드");
        reference.GetValueAsync().ContinueWith(task =>
        {
            Debug.Log("로드중");

            if (task.IsFaulted)
            {
                //Handle the error....
                Debug.Log("Handle the error");
            }
            else if (task.IsCompleted)
            {

                Debug.Log("task 성공 ");
                Debug.Log("task Status : " + task.Status);
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot);
                if (snapshot.HasChildren)
                {
                    Debug.Log("데이터 있음");
                    string saveData = snapshot.Child("Players").GetRawJsonValue();
                    Debug.Log("데이터 전환 성공");
                    Debug.Log(saveData);
                    JsonUtility.FromJsonOverwrite(saveData, this);
                    Debug.Log("데이터 Overwirte 성공");
                }
                else
                {
                    Debug.Log("데이터 없음");
                }
            }
            Debug.Log("로드완료");
        });
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Player.Clear();
    }
}
[System.Serializable]
public class PlayerContainer
{
    public UserProfile userProfiles;
    public PlayerContainer(string _Email, string _Nickname)
    {
        userProfiles = new UserProfile( _Email, _Nickname);
    }
}

[System.Serializable]
public class UserProfile
{
    public string UserEmail;
    public string UserNickname;
    
    public UserProfile(string _Email,string _Nickname)
    {
        UserEmail = _Email;
        UserNickname = _Nickname;
    }
}
