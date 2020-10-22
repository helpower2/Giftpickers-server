using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    #region SendData
    
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion
    
    #region Packets
    
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }
    
    
    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }

    /// <summary>
    /// Send a Object's updated location to all clients
    /// </summary>
    /// <param name="_transform">the Object whose location to update</param>
    public static void ObjectTransform(NetworkTransform _transform)
    {
        using (Packet _packet = new Packet((int) ServerPackets.ObjectTransform))
        {
            _packet.Write(_transform._networkId);
            _packet.Write(_transform.transform.position);
            _packet.Write(_transform.transform.rotation);
            _packet.Write(_transform.transform.localScale);
            
            SendUDPDataToAll(_packet);
        }
    }
    /// <summary>
    /// Tells the client to spawn a new prefab
    /// </summary>
    /// <param name="_prefabId">the Id of the prefab</param>
    /// <param name="_networkId">the uniqe network key</param>
    /// <param name="_transform">the transform</param>
    public static void SpawnPrefab(int _prefabId, int _networkId, Transform _transform)
    {
        using (Packet _packet = new Packet((int) ServerPackets.SpawnPrefab))
        {
            _packet.Write(_prefabId);
            _packet.Write(_networkId);
            _packet.Write(_transform.position);
            _packet.Write(_transform.rotation);
            _packet.Write(_transform.localScale);
            
            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>
    /// Tells the client to spawn a new prefab
    /// </summary>
    /// <param name="_prefabId">the Id of the prefab</param>
    /// <param name="_networkId">the uniqe network key</param>
    /// <param name="_transform">the transform</param>
    /// <param name="_toClient">the client to send it to</param>
    public static void SpawnPrefab(int _toClient, int _prefabId, int _networkId, Transform _transform)
    {
        using (Packet _packet = new Packet((int) ServerPackets.SpawnPrefab))
        {
            _packet.Write(_prefabId);
            _packet.Write(_networkId);
            _packet.Write(_transform.position);
            _packet.Write(_transform.rotation);
            _packet.Write(_transform.localScale);
            
            SendTCPData(_toClient, _packet);
        }
    }



    #endregion
    
}
