using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Network
{
    public class NetClient : MonoBehaviour
    {
        private TcpClient _client = new();

        public NetworkStream Stream => _client?.GetStream();
        public bool Connected => _client.Connected;
        
        public async void Connect(IPAddress address, int port)
        {
            try
            {
                _client = new TcpClient(address.ToString(), port);
                Debug.Log($"Connected to {address}:{port}");
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
        
        public void Disconnect()
        {
            if (!_client.Connected)
                return;
            
            _client.Close();
            Debug.Log("Disconnected");
        }
    }
}
