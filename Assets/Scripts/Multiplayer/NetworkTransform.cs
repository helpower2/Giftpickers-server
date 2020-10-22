using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTransform : MonoBehaviour
{
    public enum SendRate
    {
        EveryTick,
        WhenChanged
    }
    public int _networkId { get; private set; }
    public SendRate sendRate = SendRate.EveryTick;

    private Vector3 _position, _scale;
    private Quaternion _rotation;
    
    public void SetId(int _id)
    {
        _networkId = _id;
    }

    public void SendTransform()
    {
        ServerSend.ObjectTransform(this);
    }

    private void FixedUpdate()
    {
        switch (sendRate)
        {
            case SendRate.WhenChanged:
                if (_position != transform.position || _scale != transform.localScale || _rotation != transform.rotation)
                {
                    _position = transform.position;
                    _scale = transform.localScale;
                    _rotation = transform.rotation;
                    SendTransform();
                    
                }
                break;
            case SendRate.EveryTick:
            default:
                SendTransform();
                break;
        }
    }
}
