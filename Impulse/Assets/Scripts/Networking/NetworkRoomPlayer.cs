using Mirror;
using Networking.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class NetworkRoomPlayer : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private TMP_Text[] playerNameTexts;
        [SerializeField] private TMP_Text[] playerReadyTexts;
        [SerializeField] private Button startGameButton;
        [SerializeField] private GameObject loadingScreenUI;


        [SyncVar(hook = nameof(HandleDisplayNameChange))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChange))]
        public bool isReady = false;

        public bool _isLeader;
        public bool isLeader
        {
            get { return _isLeader; }
            set { _isLeader = value; startGameButton.gameObject.SetActive(value); }
        }

        private ImpulseNetworkManager _room;

        private ImpulseNetworkManager room
        {
            get
            {
                if (_room != null) return _room;
                return _room = NetworkManager.singleton as ImpulseNetworkManager;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerINput.DisplayName);

            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChange(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChange(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var player in room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting for Player...";
                playerReadyTexts[i].text = string.Empty;
            }
            //Could Optimize, but not important
            for (int i = 0; i < room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = "<color=blue>" + room.RoomPlayers[i].DisplayName + "</color>";
                playerReadyTexts[i].text = room.RoomPlayers[i].isReady ?
                    "<color=green>Ready</color>":
                    "<color=red>Not Ready</color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!_isLeader) return;

            startGameButton.interactable = readyToStart;
        }

        [Command]
        public void CmdSetDisplayName(string displayName)
        {
            //Name Validation
            DisplayName = displayName;
        }

        [Command]
        public void CmdReadyUp()
        {
            isReady = !isReady;

            room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            if (room.RoomPlayers[0].connectionToClient != connectionToClient) return;

            room.StartGame();
        }
    }
}
