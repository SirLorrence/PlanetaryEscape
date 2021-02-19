using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    public string DisplayName = "Loading...";
   
    private ImpulseNetworkManager _room;

    private ImpulseNetworkManager room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as ImpulseNetworkManager;
        }
    }


    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);
        room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        room.GamePlayers.Remove(this);
    }

    public void SetDisplayName(string name)
    {
        DisplayName = name;
    }
}
