using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    public Text playerName;

    public void SetPlayerInfo(MatchMessages.PlayerInfo info)
    {
        playerName.text = "Player " + info.playerIndex;
        playerName.color = info.ready ? Color.green : Color.red;
    }
}
