using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text connetionInfoText; //info Text
    public Button joinButton; //join Button

    // Start is called before the first frame update
    private void Start()
    {
        //게임 버전 = 현재 버전 
        PhotonNetwork.GameVersion = gameVersion;

        //포톤에 정보 세팅 
        PhotonNetwork.ConnectUsingSettings();

        //마스터 서버에 접속 성공하면 버튼 활성화 . . . 
        joinButton.interactable = false;

        connetionInfoText.text = "Connection To Master Server. . .";
    }

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
}
