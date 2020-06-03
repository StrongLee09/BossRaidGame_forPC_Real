using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using TMPro;

public class User
{
    public string username;
    public string email;

    public User()
    {

    }
    public User(string _username, string _email)
    {
        this.username = _username;
        this.email = _email;
    }
}

public class UsingFireBase : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://unitybossraidgame.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        User user = new User("Lee", "Test");
        string json = JsonUtility.ToJson(user);
        reference.Child("users").SetRawJsonValueAsync(json);
        //FirebaseApp.DefaultInstance.SetEditorP12FileName("unitybossraidgame-P12.p12");
        //FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("SERVICE-ACCOUNT-ID@unitybossraidgame.iam.gserviceaccount.com");
        //FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
