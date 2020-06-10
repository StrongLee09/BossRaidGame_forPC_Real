using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Public Field

    public Text connetionInfoText; //info Text
    public Button joinButton; //join Button
    public Button SetNickButton; //set Nickname Button
    public InputField NickNameInput; //NickName Input
    public GameObject InfoNickName;
    public Text InfoNickNameText;
    public Text InfoPanelText;
    public string Input;
    #endregion

    #region Private Field
    private readonly string gameVersion = "1";
    #endregion

    #region Public Methods


    //JOIN 버튼 누를시 접속시도 
    public void Connect()
    {
        
        //중복 접속 방지
        joinButton.interactable = false;

        // 버튼 누를시 연결 되있으면. . .
        if (PhotonNetwork.IsConnected)
        {
            connetionInfoText.text = "Connecting to Random Room. . .";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connetionInfoText.text = $"Offline : Connection Disabled - Try reconnecting. . .";

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SetNickName()
    {
        Input = NickNameInput.text.Trim().Replace(" ", "");
        Debug.Log(Input);
        SetNickButton.interactable = false;
        //아무것도 입력안하면 Unknown으로 
        if (Input == "")
        {
            PhotonNetwork.LocalPlayer.NickName = "Unknown";
            UserDataManager.Instance.SetUserNickName("Unknown");
            InfoNickName.SetActive(false);
            InfoPanelText.text = $"Welcome to {PhotonNetwork.LocalPlayer.NickName}";
            Debug.Log("SetUnknown");
            SetNickButton.interactable = true;
            StopCoroutine("isFindNickName");
            return;
        }
        else if(Input.Length >6){
            InfoNickNameText.text = "닉네임은 6글자이하로 정해야합니다.";
            Debug.Log("닉네임이 존나깁니다.");
            SetNickButton.interactable = true;
            StopCoroutine("isFindNickName");
            return;
        }
        else
        {
            Debug.Log("검색하기");
            FindNickName(Input);
            UserDataManager.Instance.isloading = false;
            StartCoroutine("isFindNickName");
            return;
        }
        ////입력한 닉네임으로 설정됌. . .
        //else
        //{
        //    PhotonNetwork.LocalPlayer.NickName = Input;
        //    UserDataManager.Instance.SetUserNickName(Input);
        //    InfoNickName.SetActive(false);
        //    InfoPanelText.text = $"Welcome to {PhotonNetwork.LocalPlayer.NickName}";
        //    Debug.Log("SetNickName");
        //    return;
        //}
       
    }

    public void existNick()
    {
        InfoNickNameText.text = "중복된 닉네임입니다 다시 입력해주세요.";
        SetNickButton.interactable = true;
    }

    public void nonNick()
    {
        PhotonNetwork.LocalPlayer.NickName = Input;
        InfoNickName.SetActive(false);
        InfoPanelText.text = $"Welcome to {PhotonNetwork.LocalPlayer.NickName}";
    }

    IEnumerator isFindNickName()
    {
        yield return new WaitUntil(() => UserDataManager.Instance.isloading);
        if(UserDataManager.Instance.isfind == true)
        {
            Debug.Log("존재하는 닉네임");
            existNick();
            StopCoroutine("isFindNickName");
           
        }
        else
        {
            Debug.Log("ㅊㅋㅊㅋ");
            nonNick();
            StopCoroutine("isFindNickName");
        }
    }


    #endregion

    #region Private Methods

    //private 

    private void FindNickName(string Input)=>UserDataManager.Instance.SearchUserNickName(Input);

    //닉네임 입력창
    private void InfoNickNameOn() 
    { 
        InfoNickName.SetActive(true); 
        InfoNickNameText.text = "처음이시군요 닉네임을 정해주세요...(6글자 이하)";
    }



    #endregion

    #region MonoBehaviour Callback
    // Start is called before the first frame update
    private void Start()
    {
        if (UserDataManager.Instance.isUserfirst)
        {
            //처음으로 접속한 유저면
            //닉네임 입력란 활성화 되고 버튼 누르면 SetNickName 함수 작동 ...
            Debug.Log("아이디 못찾음");
            InfoNickNameOn();
        }
        
        //게임 버전 = 현재 버전 
        PhotonNetwork.GameVersion = gameVersion;

        //포톤에 정보 세팅 
        PhotonNetwork.ConnectUsingSettings();

        //마스터 서버에 접속 성공하면 버튼 활성화 . . . 
        joinButton.interactable = false;

        connetionInfoText.text = "Connection To Master Server. . .";
    }

    #endregion

    #region Pun Callback

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connetionInfoText.text = "Online : Connetected to Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //입력이 끊긴 사유 cause

        joinButton.interactable = false;
        connetionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting. . .";

        //재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //랜덤접속 실패시 . . . 
    //빈방이 없는 경우 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connetionInfoText.text = " There is no empty room, Creating new Room.";

        //방 이름은 null 방이름을 보여주는 기능 없기때문에...
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    //자기가 방을 들어갔을때 ... 자신이 방장이 된경우 . .. 방에 참가완료된 경우
    public override void OnJoinedRoom()
    {
        connetionInfoText.text = "Connected with Room.";
        //SceneManager.LoadScene(); 나 혼자만 넘어가게됨 ㅋㅋ... 동기화가 되지않아 각자 씬이 생성됌
        PhotonNetwork.LoadLevel("Main");
    }
    #endregion




}
