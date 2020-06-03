using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;



public class GameManager : MonoBehaviourPunCallbacks
{

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    #endregion

    #region Public Fields
    public static bool GameisOver;
    public Transform[] spawnPositions;
    public Transform spawnBossPosition;
    public GameObject PlayerPrefab;
    public GameObject BossPrefab;
    #endregion

    #region Private Fields
    #endregion

    #region MonoBehaviour Callback
    // Start is called before the first frame update

    private void Awake()
    {
        //씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 
        if(Instance != this)
        {
            //파괴
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    void Start()
    {
        GameisOver = false;
        SpawnPlayer();


        if (PhotonNetwork.IsMasterClient)
        {
            //SpawnBoss

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameisOver)
        {
            return;
        }
    }

    #endregion

    #region Phton Callbacks

    public override void OnLeftRoom()
    {
        //if PhotonNetwork.Leave();
        SceneManager.LoadScene("Lobby");
    }


    #endregion

    #region Public Methods



    #endregion

    #region Private Methods
    private void SpawnPlayer()
    {
        // 외부적으론 1번 플레이어부터 세지만 내부적으론 배열을 써야하기 때문에 0부터 시작 -1  
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        //spawnpositions 0일때는 0 1일때는 1 2일때는 0 3일때는 1  outoflange case error 막아줌 
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        // A플레이어 세상에 A 케릭터 생성 B,C,D 플레이어 세상에 A플레이어 복사본이 생김 . . .
        // A플레이어 세상에서는 A케릭터는 로컬이지만 다른 세상에서는 리모트 플레이어
        // 그래서 Instantiate 하면 안됌 
        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        Debug.Log("생성");
    }

    private void SpawnBoss()
    {
        PhotonNetwork.Instantiate(BossPrefab.name, spawnBossPosition.position, Quaternion.identity);
    }
    #endregion

}
