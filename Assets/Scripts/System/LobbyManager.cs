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
    public InputField NickNameInput; //NickName Input
    public UserInfo userInfo; //user Info
    public GameObject InfoNickName;
    public Text InfoNickNameText;
    public Text InfoPanelText;

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
        string Input= NickNameInput.text.Trim().Replace(" ","");

        if (Input == "")
        {
            PhotonNetwork.LocalPlayer.NickName = "Unknown";
            userInfo.AddUser(AuthManager.User.Email, "Unknown");
            userInfo.Save();
            InfoNickName.SetActive(false);
            InfoPanelText.text = $"Welcome to {PhotonNetwork.LocalPlayer.NickName}";
            Debug.Log("SetUnknown");
        }
        else if (FindNickName(Input))
        {
            InfoNickNameText.text = "중복된 닉네임입니다. 다시 입력해주세요...";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = Input;
            userInfo.AddUser(AuthManager.User.Email, Input);
            userInfo.Save();
            InfoNickName.SetActive(false);
            InfoPanelText.text = $"Welcome to {PhotonNetwork.LocalPlayer.NickName}";
            Debug.Log("SetNickName");
        }
       
    }


    #endregion

    #region Private Methods

    //private 

    private bool FindEmail()
    {
        userInfo.Load();
        InfoNickNameText.text = "";
        for (int i=0; i < userInfo.Player.Count; i++)
        {
            if (userInfo.Player[i].userProfiles.UserEmail.Equals(AuthManager.User.Email))
            {
                PhotonNetwork.LocalPlayer.NickName = userInfo.Player[i].userProfiles.UserNickname;
                Debug.Log("아이디 찾음 ");
                return true;
            }
        }
        return false;
    }

    private bool FindNickName(string _nickname)
    {
        for (int i = 0; i < userInfo.Player.Count; i++)
        {
            if (userInfo.Player[i].userProfiles.UserNickname.Equals(_nickname))
            {
                return true;
            }
        }
        return false;
    }

    private void InfoNickNameOn() => InfoNickName.SetActive(true);

    private void OnApplicationQuit()
    {
        userInfo.Player.Clear();
    }

    #endregion

    #region MonoBehaviour Callback
    // Start is called before the first frame update
    private void Start()
    {
        
        if (!FindEmail())
        {
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
