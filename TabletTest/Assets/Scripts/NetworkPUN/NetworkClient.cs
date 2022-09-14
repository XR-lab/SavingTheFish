using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkClient : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private NetworkClientBaseData dataPrefab;
    
    private float timer = 0;
    private float timerThreshold = 30;
    private readonly int tryConnectEvery = 30;

    private PhotonTransformView transformView;
	
	void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
	
    void Start()
    {
        TryJoinRoom();
    }

    void TryJoinRoom()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connnected To Server");
        base.OnConnectedToMaster();
        TryConnectToRoom();
    }

    private void TryConnectToRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinRoom("XR-CINEKID");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Successfully created room");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();

        PhotonNetwork.Instantiate(dataPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Failed to join room: " + message);
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Failed to create room: " + message);
        base.OnCreateRoomFailed(returnCode, message);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerThreshold && !PhotonNetwork.IsConnected)
        {
            timerThreshold = timer + tryConnectEvery;
            PhotonNetwork.ConnectUsingSettings();
        }

	if (timer > timerThreshold && PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            timerThreshold = timer + tryConnectEvery;
	    TryConnectToRoom();
        }
    }

    public override void OnLeftLobby()
    {
        TryConnectToRoom();
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
