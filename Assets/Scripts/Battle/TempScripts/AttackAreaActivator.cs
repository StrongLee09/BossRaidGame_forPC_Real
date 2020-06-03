using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaActivator : MonoBehaviour
{
    Collider[] attackAreaColliders;//공격 판정 콜리더 배열

    
    void Start()
    {
        //자식 오브젝트에서 AttackArea 스크립트가 추가된 오브젝트를 찾는다.
        AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
        attackAreaColliders = new Collider[attackAreas.Length];

        for(int attackAreaCnt =0; attackAreaCnt<attackAreas.Length;attackAreaCnt++)
        {
            //attackArea 스크립트가 추가된 오브젝트의 컬라이더 배열에 저장한다.
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].GetComponent<Collider>();
            attackAreaColliders[attackAreaCnt].enabled = false; //초기값은 false;
        }

    }

    //애니메이션 이벤트의 StartAttack으로 컬라이더 유효로 한다.
    void StartAttack()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = true;
    }

    //애니메이션 이벤트의 StartAttack으로 컬라이더 무효로 한다.
    void EndAttack()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = false;
    }


}
