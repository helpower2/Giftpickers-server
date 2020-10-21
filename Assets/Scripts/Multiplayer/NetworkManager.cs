using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    public GameObject playerPrefab;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;

        Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Player>();
    }
}
