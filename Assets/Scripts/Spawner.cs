using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawns = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawns; i++)
        {
            NetworkManager.Instance.SpawnPrefab(0, new Vector3(0, 0, 0),Quaternion.identity, Vector3.one);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
