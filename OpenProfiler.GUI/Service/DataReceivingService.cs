using Microsoft.Extensions.Configuration;
using OpenProfiler.GUI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OpenProfiler.GUI.Service
{
    public class DataReceivingService : IDataReceivingService
    {
        private bool _isCollecting;
        private UdpClient _udpClient;
        private readonly IBufferService _bufferService;

        public DataReceivingService(IBufferService bufferService, IConfiguration configuration)
        {
            var listeningPort = configuration.GetValue<int>(ConfigurationKeyConstants.ListeningPort);
            _udpClient = new UdpClient(listeningPort);
            _bufferService = bufferService;


            var thread = new Thread(() =>
            {
                while (true)
                {   
                    if (_isCollecting)
                    {
                        var ipAddress = new IPEndPoint(IPAddress.Loopback, 29817);
                        var data = _udpClient.Receive(ref ipAddress);
                        var str = Encoding.UTF8.GetString(data);
                        _bufferService.Add(str);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void StartCollecting()
        {
            _isCollecting = true;
        }

        public void StopCollecting()
        {
            _isCollecting = false;
        }
    }
}
