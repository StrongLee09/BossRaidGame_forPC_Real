using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Firebase/Path")]
public class FirebasePath : ScriptableObject
{
    public string BasePath, DbVersion, ObjectTypeName;
    public bool IsPlural;

    private string FullPath 
    { get { return DbVersion + "/" + BasePath + "/" + ObjectTypeName + (IsPlural ? "s" : ""); } }

    // 데이터 베이스 루트로 부터 저장된 데이터를 가져오는 DbVersion/BasePath/ObjectTypeName 
    // 데이터가 여러개이면 복수 ObjectTypeNames <-- s가 붙는 경로
    public Firebase.Database.DatabaseReference GetReferenceFromRoot(Firebase.Database.DatabaseReference root)
    {
        var objectTypeName = ObjectTypeName + (IsPlural ? "s" : "");

        return root.Child(DbVersion).Child(BasePath).Child(ObjectTypeName);
    }
}
