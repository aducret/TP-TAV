using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Network
{
    public class Channel 
    {
        private UdpClient _client;
        private IPEndPoint _remoteEndPoint;
        
        public void Connect(string ip, int port)
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _client = new UdpClient();
        }

        public void Send(byte[] data)
        {
            if (data.Length == 0 || _remoteEndPoint == null || _client == null)
                return;

            Debug.Log("Sending data...");
            _client.Send(data, data.Length, _remoteEndPoint);
        }

        public byte[] Receive(int port)
        {
            var receiveClient = new UdpClient(port);
            var anyIp = new IPEndPoint(IPAddress.Any, 0);
            Debug.Log("Receiving data...");
            var a = receiveClient.Receive(ref _remoteEndPoint);
            Debug.Log("Received data...");
            return a;
        }

        public void Close()
        {
            if (_client == null)
                return;
            
            _client.Close();
        }
    }
}
