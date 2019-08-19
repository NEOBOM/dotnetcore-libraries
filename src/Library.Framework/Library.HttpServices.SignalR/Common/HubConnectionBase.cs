using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Otakon.Library.HttpServices.SignalR.Common
{
    public class HubConnectionBase
    {
        private HubConnection _hubConnection = null;

        public HubConnectionBase(string baseUrl)
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(baseUrl).Build();

            _hubConnection.Closed += async (error) =>
            {
                Log(error);

                await KeepAlive();
            };
        }

        protected async Task Start()
        {
            try
            {
                await _hubConnection.StartAsync().ContinueWith(async t => await KeepAlive());
                //await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        protected void On(string methodName, Action<string> handler)
        {
            try
            {
                _hubConnection.On(methodName, handler);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async void Invoke(string methodName, object content)
        {
            try
            {
                await _hubConnection.InvokeAsync(methodName, content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task KeepAlive()
        {
            while (_hubConnection.State != HubConnectionState.Connected)
            {
                try
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
        }

        private void Log(Exception ex)
        {
            //TODO: create a log
        }
    }
}
