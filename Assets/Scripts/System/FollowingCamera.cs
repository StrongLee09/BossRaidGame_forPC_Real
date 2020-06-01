using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public float distanceAway = 7f;
    public float distanceUp = 4f;

    //따라다닐 객체를 지정
    public Transform follow;
  

    // Update is called once per frame
    void LateUpdate()
    {
        //카메라의 위치를 distanceUp 만큼 위에,distanceAway 만큼 앞에 위치시켜서 따라와라
        transform.position = follow.position + Vector3.up * distanceUp
            - Vector3.forward * distanceAway; 
    }
}
