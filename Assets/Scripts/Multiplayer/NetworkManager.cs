using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NetworkManager : Singleton<NetworkManager>
{
    public GameObject playerPrefab;
    public static Dictionary<int, NetworkTransform> networkTransforms = new Dictionary<int, NetworkTransform>();
    public static List<SpawnedNetworkObjects> spawnedObjects = new List<SpawnedNetworkObjects>();
    public GenericDictionary<int, GameObject> networkPrefabs = new GenericDictionary<int, GameObject>();
    
    public struct SpawnedNetworkObjects
    {
        public int PrefabID;
        public int NetwordID;
        public GameObject Object;

        public SpawnedNetworkObjects(int _prefabID, int _networdID, GameObject _gameObject)
        {
            this.PrefabID = _prefabID;
            this.NetwordID = _networdID;
            this.Object = _gameObject;
        }
    }

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
    public void SpawnPrefab(int _prefabID, Vector3 _position, Quaternion _rotation, Vector3 _scale)
    {
        GameObject _prefab = Instantiate(networkPrefabs[_prefabID], _position, _rotation);
        _prefab.transform.localScale = _scale;
        int networdID = _prefab.GetInstanceID();
        var networkTransform = _prefab.gameObject.GetComponent<NetworkTransform>();
        if (networkTransform != null)
        {
            networkTransform.SetId(networdID);
            networkTransforms.Add(networdID, networkTransform);
        }
        spawnedObjects.Add(new SpawnedNetworkObjects(_prefabID, networdID, _prefab));
    }
}
