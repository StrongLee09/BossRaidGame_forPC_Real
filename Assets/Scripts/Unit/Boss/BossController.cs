
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum BossAttackPatern
{
    MeleeAttack,
    Skill1Attack,
    Skill2Attack,
    Skill3Attack,
    SkillRAttack,
}


public class BossController : MonoBehaviour
{
    public static BossController Instance;
    private Animator avatar;
    private Rigidbody avatarRigidbody;
    CharacterStatus Charstat;
    GameObject[] Players;
    Transform Target;
    NavMeshAgent nav;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
   
    void Start()
    {
       
        Charstat = this.GetComponent<CharacterStatus>();
        avatar = transform.GetChild(0).GetComponent<Animator>();
        avatarRigidbody = transform.GetChild(0).GetComponent<Rigidbody>();
        nav = transform.GetChild(0).GetComponent<NavMeshAgent>();
        Players = GameObject.FindGameObjectsWithTag("Player");
        InvokeRepeating("SetTarget",0,20f);
        InvokeRepeating("SelectAttackPatern", 0, 10f);
        MoveStart();
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(Target.position);
        }

        //bebug
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            MoveStop();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MoveStart();
        }

    }

    public void SetTarget()
    {
        
        int Ran = Random.Range(0, Players.Length);
        Target = Players[Ran].transform;
        Debug.Log("Target Players Number :"+Ran);
    }
    
    public void SelectAttackPatern()
    {
        int SelectNum = Random.Range(0, 5);
        BossAttackPatern ActiveState = (BossAttackPatern)SelectNum;
       
        switch (ActiveState)
        {
            case BossAttackPatern.MeleeAttack:
                Debug.Log("Attack Active");
                MoveStart();
                break;
            case BossAttackPatern.Skill1Attack:
                Debug.Log("Skill1 Active");
                MoveStop();
                Invoke("MoveStart", 2f);
                break;
            case BossAttackPatern.Skill2Attack:
                Debug.Log("Skill2 Active");
                MoveStop();
                Invoke("MoveStart", 2f);
                break;
            case BossAttackPatern.Skill3Attack:
                Debug.Log("Skill3 Active");
                MoveStop();
                Invoke("MoveStart", 2f);
                break;
            case BossAttackPatern.SkillRAttack:
                Debug.Log("SkillR Active");
                MoveStop();
                Invoke("MoveStart", 2f);
                break;
        }
    }

    public void MoveStart()
    {
        if (Charstat.HP < Charstat.MaxHP / 2)
        {
            nav.speed = 20f;
            avatar.SetBool("Run", true);
        }
        else
        {
            nav.speed = 7f;
            avatar.SetBool("Move", true);
        }
    }
    public void MoveStop()
    {
            nav.speed = 0f;
            avatar.SetBool("Run", false);
            avatar.SetBool("Move", false);
    }

    //공격구현 함수
    public void OnAttackDown()
    {


        avatar.SetBool("Attacking", true);
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
