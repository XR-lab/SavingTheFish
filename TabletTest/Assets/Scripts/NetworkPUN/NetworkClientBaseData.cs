using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkClientBaseData : MonoBehaviourPunCallbacks
{
    private bool isPlaying;
    public Action OnIsPlayingChange;

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

    public virtual bool IsPlaying
    {
        get => isPlaying;
        set
        {
            if (value != isPlaying)
            {
                OnIsPlayingChange?.Invoke();
            }
            isPlaying = value;
        }
    }
}