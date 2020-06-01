using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    CharacterStatus CharStat;
    public int attackPower;//공격력
    public Transform attacker; //공격자


    // Start is called before the first frame update
    void Start()
    {
        CharStat = this.transform.root.GetComponent<CharacterStatus>();
    }

    //맞았다.
    void OnTriggerEnter(Collider other)
    {
        if(this.transform.root.GetChild(0).tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                other.transform.root.SendMessage("TakeDamage", CharStat.Power);
            }
        }
        if (this.transform.root.GetChild(0).tag == "Enemy")
        {
            if (other.tag == "Player")
            {
                other.transform.root.SendMessage("TakeDamage", CharStat.Power);
            }
        }
    }

    ////공격 판정을 유효로 한다.
    //void OnAttack()
    //{
    //    GetComponent<Collider>().enabled = true;
    //}

    ////공격 판정을 무효로 한다.
    //void OnAttackTermination()
    //{
    //    GetComponent<Collider>().enabled = false;
    //}
}
