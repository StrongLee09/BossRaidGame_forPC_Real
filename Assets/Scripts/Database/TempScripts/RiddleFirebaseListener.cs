using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System;

public class RiddleFirebaseListener : MonoBehaviour
{
    public FirebasePath Path;

    void OnChildAdded(object sender, Firebase.Database.ChildChangedEventArgs e)
    {
        var snapshot = e.Snapshot;
       // HandleChildAdded(snapshot);
    }
    private void Start()
    {
        Path.GetReferenceFromRoot(Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference)
            .ChildAdded += OnChildAdded;
        Task.Run(() =>
        {
            var task = Path.GetReferenceFromRoot(Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference)
            .GetValueAsync();

            var result = task.Result;

            if (result.HasChildren)
            {
                foreach (var child in result.Children)
                {
                   // HandleChildAdded(child);
                }
            }
            else
            {
                //경로에 데이터 자식들이 없음
                Debug.Log("Does not have children");
            }
        });

    }
    //public void HandleChildAdded(Firebase.Database.DataSnapshot snapshot)
    //{
    //    var result = JsonUtility.FromJson<RiddleDto>(snapshot.GetRawJsonValue());
    //    var itemReceived = ItemReceived;
    //    if(itemReceived != null)
    //    {
    //        ItemReceived(this, new RiddleReceivedEventArgs(result));
    //    }
    //}

    //public event EventHandler<RiddleReceivedEventArgs> ItemReceived;
}
