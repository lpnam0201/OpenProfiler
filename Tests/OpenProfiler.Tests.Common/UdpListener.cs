using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace OpenProfiler.Tests.Common
{
    public class UdpListener
    {
        public Socket _socket;
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public UdpListener(string address, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        public void Close()
        {
            _socket.Close();
        }

        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    var txt = string.Format("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.UTF8.GetString(so.buffer, 0, bytes));
                    File.AppendAllText("udplistened.txt", txt);
                }
                catch { }

            }, state);
        }
    }
}
