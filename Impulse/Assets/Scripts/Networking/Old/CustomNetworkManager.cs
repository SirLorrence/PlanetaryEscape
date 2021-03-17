using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.Old
{
    public class CustomNetworkManager : NetworkManager
    {
        [Header("Match GUI")]
        public GameObject canvas;
        public CanvasController canvasController;

        [Header("MultiScene Setup")]
        public int instances = 3;

        [Scene]
        public string gameScene;

        bool subscenesLoaded;
        readonly List<Scene> subScenes = new List<Scene>();

        #region Unity Callbacks

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            canvasController.InitializeData();
        }

        #endregion

        #region Server System Callbacks

        // Sequential index used in round-robin deployment of players into instances and score positioning
        int clientIndex;

        /// <summary>
        /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            StartCoroutine(OnServerAddPlayerDelayed(conn));
        }

        // This delay is mostly for the host player that loads too fast for the
        // server to have subscenes async loaded from OnStartServer ahead of it.
        IEnumerator OnServerAddPlayerDelayed(NetworkConnection conn)
        {
            // wait for server to async load all subscenes for game instances
            while (!subscenesLoaded)
                yield return null;

            conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

            base.OnServerAddPlayer(conn);

            PlayerScore playerScore = conn.identity.GetComponent<PlayerScore>();
            playerScore.playerNumber = clientIndex;
            playerScore.scoreIndex = clientIndex / subScenes.Count;
            playerScore.matchIndex = clientIndex % subScenes.Count;

            clientIndex++;

            if (subScenes.Count > 0)
                SceneManager.MoveGameObjectToScene(conn.identity.gameObject, subScenes[clientIndex % subScenes.Count]);
        }

        /// <summary>
        /// Called on the server when a client is ready.
        /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            canvasController.OnServerReady(conn);
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            canvasController.OnServerDisconnect(conn);
            base.OnServerDisconnect(conn);
        }

        #endregion

        #region Client System Callbacks

        /// <summary>
        /// Called on the client when connected to a server.
        /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            canvasController.OnClientConnect(conn);
        }

        /// <summary>
        /// Called on clients when disconnected from a server.
        /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            canvasController.OnClientDisconnect();
            base.OnClientDisconnect(conn);
        }

        #endregion

        #region Start & Stop Callbacks

        /// <summary>
        /// This is invoked when a server is started - including when a host is started.
        /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
        /// </summary>
        public override void OnStartServer()
        {
            if (mode == NetworkManagerMode.ServerOnly)
                canvas.SetActive(true);

            canvasController.OnStartServer();

            StartCoroutine(ServerLoadSubScenes());
        }
    

        // We're additively loading scenes, so GetSceneAt(0) will return the main "container" scene,
        // therefore we start the index at one and loop through instances value inclusively.
        // If instances is zero, the loop is bypassed entirely.
        IEnumerator ServerLoadSubScenes()
        {
            for (int index = 1; index <= instances; index++)
            {
                yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

                Scene newScene = SceneManager.GetSceneAt(index);
                subScenes.Add(newScene);
                //Spawner.InitialSpawn(newScene);
            }

            subscenesLoaded = true;
        }

        /// <summary>
        /// This is invoked when the client is started.
        /// </summary>
        public override void OnStartClient()
        {
            canvas.SetActive(true);
            canvasController.OnStartClient();
        }

        /// <summary>
        /// This is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public override void OnStopServer()
        {
            NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
            StartCoroutine(ServerUnloadSubScenes());
            clientIndex = 0;

            canvasController.OnStopServer();
            canvas.SetActive(false);
        }
        IEnumerator ServerUnloadSubScenes()
        {
            for (int index = 0; index < subScenes.Count; index++)
                yield return SceneManager.UnloadSceneAsync(subScenes[index]);

            subScenes.Clear();
            subscenesLoaded = false;

            yield return Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient()
        {
            canvasController.OnStopClient();
            // make sure we're not in host mode
            if (mode == NetworkManagerMode.ClientOnly)
                StartCoroutine(ClientUnloadSubScenes());
        }

        // Unload all but the active scene, which is the "container" scene
        IEnumerator ClientUnloadSubScenes()
        {
            for (int index = 0; index < SceneManager.sceneCount; index++)
            {
                if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
                    yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
            }
        }

        #endregion
    }
}
