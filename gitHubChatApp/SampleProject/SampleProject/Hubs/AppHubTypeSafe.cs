using Microsoft.AspNetCore.SignalR;
using SampleProject.Models;
using SampleProject.Models.ChatApp;

namespace SampleProject.Hubs
{
    public class AppHubTypeSafe:Hub<IAppHubTypeSafe>
    {


        private readonly AppDbContext _context;

        public AppHubTypeSafe(AppDbContext context) // db
        {
            _context = context;
        }

        public async Task BroadCastMessageToIndividualClient(string message,string targetClient)
        {
            var targetNick = ClientSource.Clients.FirstOrDefault(c => c.NickName == targetClient);
           
            if (targetNick != null)
            {
                await Clients.Client(targetNick.ConnectionId).ReceiveMessageToIndividualClient(message);

            }
            else
            {
            }
        }



        public async Task GetNickName(string nickName,string fullName)
        {


            var existingClient = ClientSource.Clients.FirstOrDefault(c=>c.NickName== nickName);

            if (existingClient != null)
            {
                existingClient.ConnectionId = Context.ConnectionId; // Eski connectionId'yi güncelleiyor,isme karısmıyor
               
            }
            else
            {

                Client client = new Client
                {
                    ConnectionId = Context.ConnectionId,
                    NickName = nickName,
                    fullName = fullName
                 
                };
                ClientSource.Clients.Add(client);
            }


            await Clients.All.clientler(ClientSource.Clients);
        }


        public async override Task OnConnectedAsync()
        {
            var userLogin = Context.User?.Identity?.Name;

            var fname = _context.Users.Where(x => x.UserName == userLogin).Select(x => x.FullName).FirstOrDefault() ?? "Adsız";

            var username = Context.User?.Identity?.IsAuthenticated == true ? Context.User.Identity.Name : "Anonim";
            await Clients.Caller.connectionIdForClient(Context.ConnectionId,username,fname);
        }


        public async override Task OnDisconnectedAsync(Exception? exception)
        {

            var disconnectedClient = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (disconnectedClient != null)
            {
                ClientSource.Clients.Remove(disconnectedClient);
            }

            await Clients.All.clientler(ClientSource.Clients); 
            await base.OnDisconnectedAsync(exception); 
        }




    }
}
