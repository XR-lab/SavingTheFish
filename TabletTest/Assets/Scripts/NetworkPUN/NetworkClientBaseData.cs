using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkClientBaseData : MonoBehaviourPunCallbacks
{
    public bool IsPlaying;

    protected PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        view.RequestOwnership();
    }
}