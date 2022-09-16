using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SaveTheFishData : NetworkClientBaseData, IPunObservable
{
    private int fishSaved;
    public Action<int, int> OnFishSavedChange;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.IsPlaying);
            stream.SendNext(this.FishSaved);
        }
        else
        {
            IsPlaying = (bool)stream.ReceiveNext();
            FishSaved = (int)stream.ReceiveNext();
        }
    }

    public int FishSaved
    {
        get => fishSaved;
        set
        {
            if (value != fishSaved)
            {
                OnFishSavedChange?.Invoke(fishSaved, value);
            }
            fishSaved = value;
        }
    }
}
