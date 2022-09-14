using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SaveTheFishData : NetworkClientBaseData, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.IsPlaying);
        }
        else
        {
            IsPlaying = (bool)stream.ReceiveNext();
        }
    }
}
