using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //추후 부텅있는 Animator 가져올것
    private Animator avatar;
    private Rigidbody avatarRigidbody;
    CharacterStatus Charstat;
    void Start()
    {
        Charstat = this.GetComponent<CharacterStatus>();
        avatar = transform.GetChild(0).GetComponent<Animator>();
        avatarRigidbody = transform.GetChild(0).GetComponent<Rigidbody>();
    }

    //방향관련
    //h: Horizontal
    //v: Vertical 
    float h, v;


    //조이스틱컨트롤러에서 변경되면 호출되는 함수
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Charstat.attacking != true)
        {
            Move();
        }
    }

    void Move()
    {
        //애니메이터에 전달할 값 
        avatar.SetFloat("Speed", (h * h + v * v));
        if (avatarRigidbody)
        {
            if (h != 0f && v != 0f)
            {
                //캐릭터 방향 전환
                //애니메이터에 전달되지 않고 자체적으로 방향해결
                transform.GetChild(0).rotation =
                    Quaternion.LookRotation(new Vector3(h, 0f, v).normalized * Time.deltaTime);
                avatarRigidbody.transform.Translate(Vector3.forward * Time.deltaTime * 5f*Charstat.Speed);
            }
        }
    }

    //공격구현 함수
    public void OnAttackDown()
    {


        avatar.SetBool("Attacking", true);
        //코루틴
        // StartCoroutine(StartAttack());
    }
    public void OnAttackUp()
    {
        avatar.SetBool("Attacking", false);
    }

    public void OnSkill1Down()
    {
        avatar.SetBool("Skill1", true);
    }
    public void OnSkill1Up()
    {
        avatar.SetBool("Skill1", false);
    }

    public void OnSkill2Down()
    {
        avatar.SetBool("Skill2", true);
    }
    public void OnSkill2Up()
    {
        avatar.SetBool("Skill2", false);
    }
    public void OnSkill3Down()
    {
        avatar.SetBool("Skill3", true);
    }
    public void OnSkill3Up()
    {
        avatar.SetBool("Skill3", false);
    }

    public void OnSkillRDown()
    {
        avatar.SetBool("SkillR", true);
    }
    public void OnSkillRUp()
    {
        avatar.SetBool("SkillR", false);
    }
}
