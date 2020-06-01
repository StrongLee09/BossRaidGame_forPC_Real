using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICtrl : MonoBehaviour
{
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player1").transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.position - transform.position;
        if (dir.x > 1f)
        {
            transform.LookAt(player);
            this.gameObject.GetComponent<Rigidbody>().transform.Translate(Vector3.forward * Time.deltaTime * 5f * 2f);
        }
    }
}
