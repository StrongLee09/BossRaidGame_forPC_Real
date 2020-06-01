using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereControll : MonoBehaviour
{
    Transform target;
    public GameObject Meteor;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("SkillRPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.DOMove(target.position, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            GameObject MeteorGo = (GameObject)Instantiate(Meteor, transform.position,Meteor.transform.localRotation);
            Destroy(gameObject);
        }
    }
}
