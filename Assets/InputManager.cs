using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputManager : MonoBehaviour
{
    #region Public Field
    public PhotonView PV;
    #endregion


    Vector2 slideStartPosition;
    Vector2 prevPosition;
    Vector2 delta = Vector2.zero;
    bool moved = false;

    // Update is called once per frame
    void Update()
    {
            //슬라이드 시작 지점
            if (Input.GetButtonDown("Fire2"))
                slideStartPosition = GetCursorPosition();

        //화면 너비의 10%이상 커서를 이동시키면 슬라이드 시작으로 판단
        if (Input.GetButton("Fire2"))
        {
            if (Vector2.Distance(slideStartPosition, GetCursorPosition()) >=
                (Screen.width * 0.1f))
            {
                moved = true;
                Debug.Log("우클릭");
            }
        }

        //슬라이드가 끝났는가
        if (!Input.GetButtonUp("Fire2") && !Input.GetButton("Fire2"))
        {
            //슬라이드는 끝났다.
            Debug.Log("우클릭끝");
            moved = false;
        }

            //이동량을 구한다.
            if (moved)
                delta = GetCursorPosition() - prevPosition;
            else
                delta = Vector2.zero;


            //커서 위치를 갱신한다.
            prevPosition = GetCursorPosition();
    }

    //클릭이 되었는가
    public bool Clicked()
    {
        if (!moved && Input.GetButtonUp("Fire2"))
            return true;
        else
            return false;
    }

    //슬라이드할 때 커서 이동량
    public Vector2 GetDeltaPosition()
    {
        return delta;
    }
    //슬라이드 중인가
    public bool Moved()
    {
        return moved;
    }
    public Vector2 GetCursorPosition()
    {
        return Input.mousePosition;
    }



}
