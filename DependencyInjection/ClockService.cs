using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DependencyInjection
{
    public class ClockService : IClockService
    {
        private Timer _timer;
        private readonly IHubConnectionContext<dynamic> _clients;

        public ClockService(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public void Start()
        {
            if (_timer != null)
            {
                return;
            }

            _timer = new Timer(Fire, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private void Fire(object sender)
        {
            _clients.All.tick(DateTimeOffset.UtcNow);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}