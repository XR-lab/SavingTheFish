using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PhotonView))]
public class TrashGameNetworking : NetworkClientBaseData, IPunObservable
{
    private float totalTrashCatched;
    public Action<float, float> OnTotalTrashCatchedChange;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.IsPlaying);
            stream.SendNext(this.TotalTrashCatched);
        }
        else
        {
            IsPlaying = (bool)stream.ReceiveNext();
            TotalTrashCatched = (float) stream.ReceiveNext();
        }
    }

    public float TotalTrashCatched
    {
        get => totalTrashCatched;
        set
        {
            if (Math.Abs(totalTrashCatched - value) > 0.05)
            {
                OnTotalTrashCatchedChange?.Invoke(totalTrashCatched, value);
            }
            totalTrashCatched = value;
        }
    }
}