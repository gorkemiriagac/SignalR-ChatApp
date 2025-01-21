using Microsoft.AspNetCore.SignalR;

namespace SampleProject.Hubs
{
    public class AppHub:Hub
    {

        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageForAllClient", message);

        }

    }
}
