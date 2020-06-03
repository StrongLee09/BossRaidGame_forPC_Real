using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using System.Threading.Tasks;

[CreateAssetMenu(menuName ="Firebase/Repository")]
public class FirebaseRepo : ScriptableObject
{
    
    public FirebasePath Path;

    public Task Push(object item)
    {
        if(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser != null)
        { //Do not commit it if the user is not logged in 유저가 로그인하지않으면 접근 못함 
            return Path.GetReferenceFromRoot(Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference)
                .Push()
                .SetRawJsonValueAsync(JsonUtility.ToJson(item));
        }

        return Task.FromResult(new Object());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
