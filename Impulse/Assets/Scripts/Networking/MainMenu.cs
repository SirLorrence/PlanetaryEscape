using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
