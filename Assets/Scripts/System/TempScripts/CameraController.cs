using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singlton
    public static CameraController instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion


    public GameObject Cam1;
    public GameObject Cam2;
    public GameObject Cam3;
    Vector3 tempVec;

    // Start is called before the first frame update
    void Start()
    {
        Cam1On();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
            //Cam2.transform.position.Set(tempVec.x,tempVec.y,tempVec.z);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            Cam3.SetActive(false);
            tempVec = Cam2.transform.position;
            //MoveCamera();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Cam1.SetActive(false);
            Cam2.SetActive(false);
            Cam3.SetActive(true);
            //MoveCamera();
        }
    }

    public void Cam1On()
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
        Cam3.SetActive(false);
    }

    public void Cam2On()
    {
        Cam1.SetActive(false);
        Cam2.SetActive(true);
        Cam3.SetActive(false);
    }
    public void Cam3On()
    {
        Cam1.SetActive(false);
        Cam2.SetActive(false);
        Cam3.SetActive(true);
    }
}
