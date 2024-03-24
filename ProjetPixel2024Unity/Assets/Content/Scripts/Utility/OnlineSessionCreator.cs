using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using FishNet.Transporting;

public class OnlineSessionCreator : MonoBehaviour
{
    private NetworkManager _networkManager;

    // Start is called before the first frame update
    void Start()
    {
        // initialize network manager
        _networkManager = FindObjectOfType<NetworkManager>();

        // start the session
        _networkManager.ServerManager.StartConnection();
        _networkManager.ClientManager.StartConnection();
    }
}
