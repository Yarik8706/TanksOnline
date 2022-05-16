using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UI;

public class LanManager : MonoBehaviour {
    // Addresses of the computer (Ethernet, WiFi, etc.)
    public List<string> LocalAddresses { get; private set; }
    public List<string> LocalSubAddresses { get; private set; }
    
    public List<string> Addresses { get; private set; }
    public List<string> ServerNames { get; private set; }

    public bool IsSearching { get; private set; }

    private Socket _socketServer;
    private Socket _socketClient;
    private MessageHandler _messageHandler;
    private EndPoint _remoteEndPoint;

    private void Start()
    {
        _messageHandler = GetComponent<MessageHandler>();
        Addresses = new List<string>();
        ServerNames = new List<string>();
        LocalAddresses = new List<string>();
        LocalSubAddresses = new List<string>();
    }

    public void StartServer(int port)
    {
        if (_socketServer != null || _socketClient != null) return;
        try {
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
 
            if (_socketServer == null) {
                _messageHandler.AddMessage("SocketServer creation failed");
                return;
            }
 
            // Check if we received pings
            _socketServer.Bind(new IPEndPoint(IPAddress.Any, port));
 
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
 
            _socketServer.BeginReceiveFrom(new byte[1024], 0, 1024, SocketFlags.None,
                ref _remoteEndPoint, _ReceiveServer, null);
            _messageHandler.AddMessage("Start server on port " + port);
        } catch (Exception ex) {
            _messageHandler.AddMessage(ex.Message);
        }
    }
 
    public void CloseServer()
    {
        if (_socketServer == null) return;
        _socketServer.Close();
        _socketServer = null;
    }
 
    public void StartClient(int port)
    {
        if (_socketServer != null || _socketClient != null) return;
        try {
            _socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
 
            if (_socketClient == null) {
                _messageHandler.AddMessage("SocketClient creation failed");
                return;
            }
 
            // Check if we received response from a remote (server)
            _socketClient.Bind(new IPEndPoint(IPAddress.Any, port));
 
            _socketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            _socketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
 
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
 
            _socketClient.BeginReceiveFrom(new byte[1024], 0, 1024, SocketFlags.None,
                ref _remoteEndPoint, _ReceiveClient, null);
            _messageHandler.AddMessage("Start client on port " + port);
        } catch (Exception ex) {
            _messageHandler.AddMessage(ex.Message);
        }
    }
 
    public void CloseClient()
    {
        if (_socketClient == null) return;
        _socketClient.Close();
        _socketClient = null;
    }
 
    public IEnumerator SendPing(int port) {
        Addresses.Clear();
        ServerNames.Clear();

        if (_socketClient == null) yield break;
        const int maxSend = 7;

        IsSearching = true;
        
        _messageHandler.AddMessage("Start ping");
        
        // Send several pings just to be sure (a ping can be lost!)
        for (int i = 0 ; i < maxSend ; i++) {
 
            // For each address that this device has
            foreach (string subAddress in LocalSubAddresses) {
                var destinationEndPoint = new IPEndPoint(IPAddress.Parse(subAddress + ".255"), port);
                var str = Encoding.ASCII.GetBytes("ping:");
 
                _socketClient.SendTo(str, destinationEndPoint);
                _messageHandler.AddMessage("Send ping");

                yield return new WaitForSeconds(0.1f);
            }
        }
        
        _messageHandler.AddMessage("End ping");
        
        IsSearching = false;
    }
 
    private void _ReceiveServer(IAsyncResult ar)
    {
        if (_socketServer == null) return;
        try {
            int size = _socketServer.EndReceiveFrom(ar, ref _remoteEndPoint);
            byte[] str = Encoding.ASCII.GetBytes("pong:"+LocalAddresses[0]+":"+LoginUI.playerName);
            
            _socketServer.SendTo(str, _remoteEndPoint);
            
            _socketServer.BeginReceiveFrom(new byte[1024], 0, 1024, SocketFlags.None,
                ref _remoteEndPoint, _ReceiveServer, null);
        } catch (Exception ex) {
            _messageHandler.AddMessage(ex.ToString());
        }
    }

    private static string GetSendedMessage(Socket socket)
    {
        byte[] bytesReceived = new byte[0x400];
        int bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
        return Encoding.ASCII.GetString(bytesReceived, 0, bytes);
    }
 
    private void _ReceiveClient(IAsyncResult ar)
    {
        if (_socketClient == null) return;
        try {
            var size = _socketClient.EndReceiveFrom(ar, ref _remoteEndPoint);
            var message = GetSendedMessage(_socketClient);

            var messageParts = message.Split(':');
            
            if (!Addresses.Contains(messageParts[1]) && messageParts[0].Contains("pong")) 
            {
                Addresses.Add(messageParts[1]);
                ServerNames.Add(messageParts[2]);
            }
            _messageHandler.AddMessage("Get message: " + message);
 
            _socketClient.BeginReceiveFrom(new byte[1024], 0, 1024, SocketFlags.None,
                ref _remoteEndPoint, _ReceiveClient, null);
        } catch (Exception ex) {
            Debug.Log(ex.ToString());
        }
    }
 
    public void ScanHost() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
 
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
            var address = ip.ToString();
            var subAddress = address.Remove(address.LastIndexOf('.'));
 
            LocalAddresses.Add(address);
 
            if (!LocalSubAddresses.Contains(subAddress)) {
                LocalSubAddresses.Add(subAddress);
            }
        }
    }
}