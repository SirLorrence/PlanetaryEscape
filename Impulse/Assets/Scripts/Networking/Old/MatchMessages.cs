using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchMessages : MonoBehaviour
{
    /// <summary>
    /// Match message to be sent to the server
    /// </summary>
    public struct ServerMatchMessage : NetworkMessage
    {
        public ServerMatchOperation serverMatchOperation;
        public Guid matchId;
    }

    /// <summary>
    /// Match message to be sent to the client
    /// </summary>
    public struct ClientMatchMessage : NetworkMessage
    {
        public ClientMatchOperation clientMatchOperation;
        public Guid matchId;
        public MatchInfo[] matchInfos;
        public PlayerInfo[] playerInfos;
    }

    /// <summary>
    /// Information about a match
    /// </summary>
    [Serializable]
    public struct MatchInfo
    {
        public Guid matchId;
        public byte players;
        public byte maxPlayers;
    }

    /// <summary>
    /// Information about a player
    /// </summary>
    [Serializable]
    public struct PlayerInfo
    {
        public int playerIndex;
        public bool ready;
        public Guid matchId;
    }

    [Serializable]
    public struct MatchPlayerData
    {
        public int playerIndex;
        public int score;
    }

    /// <summary>
    /// Match operation to execute on the server
    /// </summary>
    public enum ServerMatchOperation : byte
    {
        None,
        Create,
        Cancel,
        Start,
        Join,
        Leave,
        Ready
    }

    /// <summary>
    /// Match operation to execute on the client
    /// </summary>
    public enum ClientMatchOperation : byte
    {
        None,
        List,
        Created,
        Cancelled,
        Joined,
        Departed,
        UpdateRoom,
        Started
    }
}
