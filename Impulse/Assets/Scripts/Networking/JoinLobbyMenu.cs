using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private ImpulseNetworkManager networkManager;

    [Header("UI")]
    [SerializeField] private GameObject landingPage;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private Button joinButton;

    private void OnEnable()
    {
        ImpulseNetworkManager.OnClientConnected += HandleClientConnected;
        ImpulseNetworkManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        ImpulseNetworkManager.OnClientConnected -= HandleClientConnected;
        ImpulseNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPage.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
