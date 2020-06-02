using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    //추후 부텅있는 Animator 가져올것
    public Animator AN;
    public Rigidbody RB;
    public PhotonView PV;
    public Text NickNameText;
    public Image HealthImage;

    //방향관련
    //h: Horizontal
    //v: Vertical 
    float h, v;


    private void Awake()
    {
        // 닉네임
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
    }
    void Start()
    {
    }

    


    //조이스틱컨트롤러에서 변경되면 호출되는 함수
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Charstat.attacking != true)
        //{
        //    Move();
        //}
        if (PV.IsMine)
        {
            Move();
        }
    }

    void Move()
    {
        
        //애니메이터에 전달할 값 
        AN.SetFloat("Speed", (h * h + v * v));
        if (RB)
        {
            if (h != 0f && v != 0f)
            {
                //캐릭터 방향 전환
                //애니메이터에 전달되지 않고 자체적으로 방향해결
                transform.GetChild(0).rotation =
                    Quaternion.LookRotation(new Vector3(h, 0f, v).normalized * Time.deltaTime);
                RB.transform.Translate(Vector3.forward * Time.deltaTime * 5f );//Charstat.Speed);
            }
        }
    }

    [PunRPC]
    //void 


    //공격구현 함수
    public void OnAttackDown()
    {
        AN.SetBool("Attacking", true);
    }
    public void OnAttackUp()
    {
        AN.SetBool("Attacking", false);
    }

    public void OnSkill1Down()
    {
        AN.SetBool("Skill1", true);
    }
    public void OnSkill1Up()
    {
        AN.SetBool("Skill1", false);
    }

    public void OnSkill2Down()
    {
        AN.SetBool("Skill2", true);
    }
    public void OnSkill2Up()
    {
        AN.SetBool("Skill2", false);
    }
    public void OnSkill3Down()
    {
        AN.SetBool("Skill3", true);
    }
    public void OnSkill3Up()
    {
        AN.SetBool("Skill3", false);
    }

    public void OnSkillRDown()
    {
        AN.SetBool("SkillR", true);
    }
    public void OnSkillRUp()
    {
        AN.SetBool("SkillR", false);
    }
}
