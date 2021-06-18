using System;
using System.Net.Sockets;

namespace CoreCard.Tesla.NetworkInterface.Communication
{
    public interface ISocketClient : IDisposable
    {
        void Connect();
        void Disconnect();
        void Send(byte[] data);
        public Action<Socket> OnConnected { get; set; }
        public Action<Socket> OnDisconnected { get; set; }
        public Action<byte[]> OnReceive { get; set; }
        public Action<SocketAsyncEventArgs> OnSent { get; set; }
    }
}