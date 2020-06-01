using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Info : MonoBehaviour
{
    //체력 
    public float HP = 500000;

    public float MaxHP = 500000;
    //공격력

    public float Power = 500;
    public float SkillPower = 100;
    //방어력 
    public float Defense = 50;
    //스피드
    public float Speed = 1;
    public float AttackSpeed = 1;
    //경험치
    public float Experience = 0;
    //레벨
    public int Level = 1;
    //플레이어 이름 
    public string CharacterName = "Boss1";
    public string Type = "Enemy";

    CharacterStatus CharStat;


    // Start is called before the first frame update
    void Start()
    {
        CharStat = this.transform.root.GetComponent<CharacterStatus>();
        Initialize();
    }

    void Initialize()
    {
        CharStat.Initialize(HP, MaxHP, Power
            , SkillPower, Defense, Speed
            , AttackSpeed, Experience, Level
            , CharacterName, Type);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
