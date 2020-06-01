using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrenSkill3Controller : MonoBehaviour
{
    CharacterStatus CharStat;
    private void Start()
    {
        CharStat = GameObject.Find("Grenadier").transform.root.GetComponent<CharacterStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Vector3 dir = other.GetComponent<Transform>().position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
            other.transform.root.SendMessage("TakeDamage", CharStat.SkillPower * 10 * transform.localScale.x);//GetComponent<CharacterStatus>().TakeDamage(CharStat.SkillPower * 10 * transform.localScale.x);
        }
        if(other.tag == "Bullet")
        {
            Destroy(other);
        }
    }
}
