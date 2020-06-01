using DG.Tweening;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrendierInfo : MonoBehaviour
{
    [Header("GrendierStat")]
    //체력 
    public float HP = 5000;

    public float MaxHP = 5000;
    //공격력

    public float Power = 100;
    public float SkillPower = 10;
    //방어력 
    public float Defense = 10;
    //스피드
    public float Speed = 1;
    public float AttackSpeed = 1;
    //경험치
    public float Experience = 0;
    //레벨
    public int Level = 1;
    //플레이어 이름 
    public string CharacterName = "DollFight";
    public string Type = "Player";
    CharacterStatus CharStat;
    public Transform FirePoint;
    public float Skill1Radius = 30f;
    public GameObject GrenSkill1;
    public GameObject GrenSkillR;
    public GameObject GrenSkill3;

    // Start is called before the first frame update
    void Start()
    {
        CharStat = this.transform.root.GetComponent<CharacterStatus>();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        CharStat.Initialize(HP, MaxHP, Power
            , SkillPower, Defense, Speed
            , AttackSpeed, Experience, Level
            , CharacterName, Type);
    }

    void SetAttackStart()
    {
        CharStat.StartAttacking();
    }
    void SetAttackEnd()
    {
        CharStat.EndAttacking();
    }

    void Skill1()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, Skill1Radius);
        foreach(Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                collider.transform.root.SendMessage("TakeDamage", CharStat.SkillPower * 50*transform.localScale.x);
                GameObject Skill1Go = (GameObject)Instantiate(GrenSkill1, transform.position, transform.rotation);
                Skill1Go.transform.DOScale(transform.localScale,0f);
                GrenSkill1Controller Grenskill1Go = Skill1Go.GetComponent<GrenSkill1Controller>();

                if(Grenskill1Go != null)
                {
                    Grenskill1Go.Seek(collider.transform);
                }
            }
        }
    }

    void Skill2()
    {
        transform.DOPunchScale(new Vector3(1,1,1), 1f);
        transform.DOShakeScale(5f,1f);
        transform.DOScale(new Vector3(transform.localScale.x+1f, transform.localScale.x + 1f, transform.localScale.x + 1f), 1f);
        Skill1Radius = 10f*transform.localScale.x;
        CharStat.SetStat(0, 0, transform.localScale.x * 100, 0, 0, 0, 0, 0, true);
    }
    void Skill3()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Skill1Radius / 2);
        foreach(Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Vector3 dir = collider.GetComponent<Transform>().position - transform.position;
                collider.GetComponent<Rigidbody>().AddForce(dir * 40f, ForceMode.Impulse);
            }
        }
        Invoke("Skill3Go", 0.5f);
        
    }
    void Skill3Go()
    {
        GameObject Skill3Go = (GameObject)Instantiate(GrenSkill3, FirePoint.position, transform.rotation);
        Skill3Go.transform.DOScale(Skill3Go.transform.localScale+transform.localScale*10, 2f);
        Destroy(Skill3Go, 2f);
    }
    void SkillRstart()
    {
        CameraController.instance.Cam2On();
    }

    void SkillR()
    {
        GameObject SkillRGo = (GameObject)Instantiate(GrenSkillR,FirePoint.position, transform.rotation);
        CameraController.instance.Cam3On();
        Invoke("Cam1On", 5.5f);
    }
    void Cam1On()
    {
        CameraController.instance.Cam1On();
        SetAttackEnd();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Skill1Radius);
    }
}
