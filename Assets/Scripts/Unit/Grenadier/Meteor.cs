using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Transform Boss;

    public float SkillRRadius = 100f;
    public float force = 1500f;
    public GameObject MeteorEffect;
    CharacterStatus CharStat;
    // Start is called before the first frame update
    void Start()
    {
        CharStat = GameObject.Find("Grenadier").transform.root.GetComponent<CharacterStatus>();
        Boss = GameObject.Find("Boss").transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.DOMove(Boss.transform.position, 7f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Explode();
            GameObject MeteorGo = (GameObject)Instantiate(MeteorEffect, transform.position, transform.rotation);
            GameObject MeteorGo1 = (GameObject)Instantiate(MeteorEffect, transform.position, transform.rotation);
            Destroy(MeteorGo, 5f);
            Destroy(MeteorGo1, 5f);
            Destroy(this.gameObject);
        }
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, SkillRRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                rb.AddExplosionForce(force, transform.position, SkillRRadius);
            }
            if (collider.tag == "Enemy")
            {
                if (collider.transform.root.GetComponent<CharacterStatus>() != null)
                {
                    collider.transform.root.SendMessage("TakeDamage", CharStat.SkillPower * 500);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SkillRRadius);
    }
}
