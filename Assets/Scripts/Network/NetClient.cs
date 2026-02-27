using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utilities;
using Utilities.Behaviour;

namespace Network
{
    public sealed class NetClient : MonoSingleton<NetClient>
    {
        private TcpClient _client = new();
        
        #region Events

        [CanBeNull] public event Action OnConnected;
        [CanBeNull] public event Action<Exception> OnConnectionFailed;
        [CanBeNull] public event Action OnDisconnected;

        #endregion

        #region Properties

        public NetworkStream Stream => _client?.GetStream();
        public bool Connected => _client.Connected;

        #endregion
        
        public async Task Connect(IPAddress address, int port)
        {
            try
            {
                if (Connected)
                    return;

                _client = new TcpClient();
                
                await _client.ConnectAsync(address, port);
                OnConnected?.Invoke();
            }
            catch (Exception exception)
            {
                OnConnectionFailed?.Invoke(exception);
            }
        }
        
        public void Disconnect()
        {
            if (!_client.Connected)
                return;
            
            _client.Close();
            
            OnDisconnected?.Invoke();
        }
    }
}
