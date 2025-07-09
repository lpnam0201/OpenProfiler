using OpenProfiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenProfiler.Messaging
{
    public class UdpMessageDispatcher
    {
        private UdpClient _udpClient;

        public UdpMessageDispatcher(Options options)
        {
            _udpClient = new UdpClient(options.Host, options.Port);
        }

        public void Dispatch(List<ProfilerEvent> profilerEvents)
        {
            var serialized = JsonSerializer.Serialize(profilerEvents);
            var bytes = Encoding.UTF8.GetBytes(serialized);
            // Fire and forget, do not wait 
            _udpClient.SendAsync(bytes, bytes.Length);
        }
    }
}
