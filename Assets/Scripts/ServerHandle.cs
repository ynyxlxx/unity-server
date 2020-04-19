﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle {

    public static void WelcomeReceived (int _fromClient, Packet _packet) {
        int _clientIdCheck = _packet.ReadInt ();
        string _username = _packet.ReadString ();

        Debug.Log ($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and now is playing {_fromClient}.");
        if (_fromClient != _clientIdCheck) {
            Debug.Log ($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }

        //send the player into the Game
        Server.clients[_fromClient].SendIntoGame (_username);
    }

    public static void UDPTestReceived (int _fromClient, Packet _packet) {
        string _msg = _packet.ReadString ();

        Debug.Log ($"Received packet via UDP. Contains message: {_msg}");
    }

    public static void PlayerMovement (int _fromClient, Packet _packet) {
        bool[] _inputs = new bool[_packet.ReadInt ()];
        for (int i = 0; i < _inputs.Length; i++) {
            _inputs[i] = _packet.ReadBool ();
        }
        Quaternion _rotation = _packet.ReadQuaternion ();

        Server.clients[_fromClient].player.SetInput (_inputs, _rotation);
    }
}