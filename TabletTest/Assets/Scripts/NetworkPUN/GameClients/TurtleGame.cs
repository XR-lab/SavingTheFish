using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurtleGame : NetworkClientBaseData, IPunObservable
{
    private int turtlesSaved;
    public Action<int, int> OnTurtlesSavedChange;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.IsPlaying);
            stream.SendNext(this.turtlesSaved);
        }
        else
        {
            IsPlaying = (bool)stream.ReceiveNext();
            TurtlesSaved = (int)stream.ReceiveNext();
        }
    }

    public int TurtlesSaved
    {
        get => turtlesSaved;
        set
        {
            if (turtlesSaved != value)
            {
                OnTurtlesSavedChange?.Invoke(turtlesSaved, value);
            }
            turtlesSaved = value;
        }
    }
}
