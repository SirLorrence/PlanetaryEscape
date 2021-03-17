using UnityEngine;

namespace Networking
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private ImpulseNetworkManager networkManager;

        [Header("UI")]
        [SerializeField] private GameObject landingPage;

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPage.SetActive(true);
        }
    }
}
