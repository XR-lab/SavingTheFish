using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CoralMinerGame : NetworkClientBaseData, IPunObservable
{
    private int robotsDestroyed;
    public Action<int, int> OnRobotsDestroyedChange;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.IsPlaying);
            stream.SendNext(this.RobotsDestroyed);
        }
        else
        {
            IsPlaying = (bool)stream.ReceiveNext();
            RobotsDestroyed = (int)stream.ReceiveNext();
        }
    }

    public int RobotsDestroyed
    {
        get => robotsDestroyed;
        set
        {
            if (robotsDestroyed != value)
            {
                OnRobotsDestroyedChange?.Invoke(robotsDestroyed, value);
            }
            robotsDestroyed = value;
        }
    }
}