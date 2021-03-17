using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Networking.Old
{
    [RequireComponent(typeof(NetworkMatchChecker))]
    public class MatchController : NetworkBehaviour
    {
        internal readonly SyncDictionary<NetworkIdentity, MatchMessages.MatchPlayerData> matchPlayerData = new SyncDictionary<NetworkIdentity, MatchMessages.MatchPlayerData>();

        bool playAgain = false;

        [Header("GUI References")]
        public CanvasGroup canvasGroup;
        public Text gameText;

        [Header("Debug - Do Not Modify")]
        public CanvasController canvasController;
        public NetworkIdentity player1;
        public NetworkIdentity player2;
        public NetworkIdentity startingPlayer;

        public NetworkIdentity currentPlayer;

        void Awake()
        {
            canvasController = FindObjectOfType<CanvasController>();
        }

        public override void OnStartServer()
        {
            StartCoroutine(AddPlayersToMatchController());
        }

        // For the SyncDictionary to properly fire the update callback, we must
        // wait a frame before adding the players to the already spawned MatchController
        IEnumerator AddPlayersToMatchController()
        {
            yield return null;

            matchPlayerData.Add(player1, new MatchMessages.MatchPlayerData { playerIndex = CanvasController.playerInfos[player1.connectionToClient].playerIndex });
            matchPlayerData.Add(player2, new MatchMessages.MatchPlayerData { playerIndex = CanvasController.playerInfos[player2.connectionToClient].playerIndex });
        }


        public override void OnStartClient()
        {
            //matchPlayerData.Callback += UpdateWins;

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        // Assigned in inspector to ReplayButton::OnClick
        [Client]
        public void RequestPlayAgain()
        {
            CmdPlayAgain();
        }

        [Command(ignoreAuthority = true)]
        public void CmdPlayAgain(NetworkConnectionToClient sender = null)
        {
            if (!playAgain)
            {
                playAgain = true;
            }
            else
            {
                playAgain = false;
                RestartGame();
            }
        }

        [Server]
        public void RestartGame()
        {
            NetworkIdentity[] keys = new NetworkIdentity[matchPlayerData.Keys.Count];
            matchPlayerData.Keys.CopyTo(keys, 0);

            foreach (NetworkIdentity identity in keys)
            {
                MatchMessages.MatchPlayerData mpd = matchPlayerData[identity];
                mpd.score = 0;
                matchPlayerData[identity] = mpd;
            }

            RpcRestartGame();

            startingPlayer = startingPlayer == player1 ? player2 : player1;
            currentPlayer = startingPlayer;
        }

        [ClientRpc]
        public void RpcRestartGame()
        {
        }

        // Assigned in inspector to BackButton::OnClick
        [Client]
        public void RequestExitGame()
        {
            CmdRequestExitGame();
        }

        [Command(ignoreAuthority = true)]
        public void CmdRequestExitGame(NetworkConnectionToClient sender = null)
        {
            StartCoroutine(ServerEndMatch(sender, false));
        }

        public void OnPlayerDisconnected(NetworkConnection conn)
        {
            // Check that the disconnecting client is a player in this match
            if (player1 == conn.identity || player2 == conn.identity)
            {
                StartCoroutine(ServerEndMatch(conn, true));
            }
        }

        public IEnumerator ServerEndMatch(NetworkConnection conn, bool disconnected)
        {
            canvasController.OnPlayerDisconnected -= OnPlayerDisconnected;

            RpcExitGame();

            // Skip a frame so the message goes out ahead of object destruction
            yield return null;

            // Mirror will clean up the disconnecting client so we only need to clean up the other remaining client.
            // If both players are just returning to the Lobby, we need to remove both connection Players

            if (!disconnected)
            {
                NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
                CanvasController.waitingConnections.Add(player1.connectionToClient);

                NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
                CanvasController.waitingConnections.Add(player2.connectionToClient);
            }
            else if (conn == player1.connectionToClient)
            {
                // player1 has disconnected - send player2 back to Lobby
                NetworkServer.RemovePlayerForConnection(player2.connectionToClient, true);
                CanvasController.waitingConnections.Add(player2.connectionToClient);
            }
            else if (conn == player2.connectionToClient)
            {
                // player2 has disconnected - send player1 back to Lobby
                NetworkServer.RemovePlayerForConnection(player1.connectionToClient, true);
                CanvasController.waitingConnections.Add(player1.connectionToClient);
            }

            // Skip a frame to allow the Removal(s) to complete
            yield return null;

            // Send latest match list
            canvasController.SendMatchList();

            NetworkServer.Destroy(gameObject);
        }

        [ClientRpc]
        public void RpcExitGame()
        {
            canvasController.OnMatchEnded();
        }
    }
}
