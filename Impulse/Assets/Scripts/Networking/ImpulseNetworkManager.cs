using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class ImpulseNetworkManager : NetworkManager
    {
        [SerializeField] private int minPlayers = 1;
        [Scene] [SerializeField] private string menuScene;
    
        [Header("Room Prefabs")]
        [SerializeField] private NetworkRoomPlayer roomPlayerPrefab;

        [Header("Game Prefabs")]
        [SerializeField] private NetworkGamePlayer gamePlayerPrefab;
        [SerializeField] private GameObject playerSpawnSystem;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();
        public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();
        //public List<NetworkSpectatorPlayer> SpectatorPlayers { get; } = new List<NetworkRoomPlayer>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            //If more then max players, disconnect
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            //If game is in progress it is unjoinable
            if (SceneManager.GetActiveScene().name != Path.GetFileNameWithoutExtension(menuScene))
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene))
            {
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.isLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayer>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        public bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) return false;

            foreach (var player in RoomPlayers)
            {
                if (!player.isReady) return false;
            }
            return true;
        }
   
        public void StartGame()
        {
            if (SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene))
            {
                if (!IsReadyToStart()) return;

                ServerChangeScene("PVE_MAP1");
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().name == Path.GetFileNameWithoutExtension(menuScene) && newSceneName.StartsWith("PVE_MAP"))
            {
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                    gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);

                }
            }
            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            //if (sceneName.StartsWith("PVE_MAP"))
            //{
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
            //}
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }
    }
}
