using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenSkill1Controller : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;


    public void Seek(Transform _target)
    {
        target = _target;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            target.GetComponent<Rigidbody>().AddForce(dir*120f,ForceMode.Impulse);
            return;
        }

        
        if (dir.magnitude >= distanceThisFrame)
        {
           
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }

    }

    void HitTarget()
    {
        Destroy(gameObject,3f);
    }
}
