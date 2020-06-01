using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public enum Characters
{
    Grenadier,
    Ellen,
    Grunt,
    SpaceSolider
}

public class CharacterStatus : MonoBehaviour
{
    
    private CharacterStatus instance;
    

    //체력 
    public float HP
    {
        get;
        private set;
    }
    
    public float MaxHP
    {
        get;
        private set;
    }
    //공격력

    public float Power {
        get;
        private set;
    }
    public float SkillPower
    {
        get;
        private set;
    }
    //방어력 
    public float Defense
    {
        get;
        private set;
    }
    //스피드
    public float Speed
    {
        get;
        private set;
    }
    public float AttackSpeed
    {
        get;
        private set;
    }
    //경험치
    public float Experience
    {
        get;
        private set;
    }
    //레벨
    public int Level
    {
        get;
        private set;
    }
    //플레이어 이름 
    public string CharacterName
    {
        get;
        private set;
    }
    public string Type
    {
        get;
        private set;
    }

    public bool attacking;
    public void Initialize(float _HP, float _MaxHP, float _Power
                       , float _SkillPower, float _Defense, float _Speed
                       , float _AttackSpeed, float _Experience, int _Level
                       , string _CharacterName, string _Type)
    {
        this.HP = _HP;
        this.MaxHP = _MaxHP;
        this.Power = _Power;
        this.SkillPower = _SkillPower;
        this.Defense = _Defense;
        this.Speed = _Speed;
        this.AttackSpeed = _AttackSpeed;
        this.Experience = _Experience;
        this.Level = _Level;
        this.CharacterName = _CharacterName;
        this.Type = _Type;
    }
    // bool Add 1 일시 증가 0일시 감소
    public void SetStat(float _HP, float _MaxHP, float _Power
                       , float _SkillPower, float _Defense, float _Speed
                       , float _AttackSpeed, float _Experience, bool add)// bool Add 1 일시 증가 0일시 감소
    {
        // bool Add 1 일시 증가 0일시 감소
        if (add)
        {
            this.HP += _HP;
            this.MaxHP += _MaxHP;
            this.Power += _Power;
            this.SkillPower += _SkillPower;
            this.Defense += _Defense;
            this.Speed += _Speed;
            this.AttackSpeed += _AttackSpeed;
            this.Experience += _Experience;
        }
        else
        {
            this.HP -= _HP;
            this.MaxHP -= _MaxHP;
            this.Power -= _Power;
            this.SkillPower -= _SkillPower;
            this.Defense -= _Defense;
            this.Speed -= _Speed;
            this.AttackSpeed -= _AttackSpeed;
            this.Experience -= _Experience;
        }
    }
    public void LevelUP(float _MaxHP, float _Power, float _SkillPower
                       , float _Defense, float _Speed, float _AttackSpeed)
    {

        this.MaxHP += _MaxHP;
        this.Power += _Power;
        this.SkillPower += _SkillPower;
        this.Defense += _Defense;
        this.Speed += _Speed;
        this.AttackSpeed += _AttackSpeed;
        this.Experience = 0;
        this.Level++;
        this.HP = MaxHP;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    public void StartAttacking()
    {
        attacking = true;
    }
    public void EndAttacking()
    {
        attacking = false;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        if(dmg-Defense < 0)
        {
            return;
        }
        HP -= dmg-Defense;
    }
    public void Heal (int amount)
    {
        HP += amount;
        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
    }
}
