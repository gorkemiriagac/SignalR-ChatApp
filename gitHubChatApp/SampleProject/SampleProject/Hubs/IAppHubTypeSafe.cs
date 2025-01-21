using SampleProject.Models.ChatApp;

namespace SampleProject.Hubs
{
    public interface IAppHubTypeSafe
    {

        Task ReceiveMessageToAllClient(string message); 
        Task ReceiveMessageToIndividualClient(string message); // Individual client

        Task connectionIdForClient(string connectionId,string username,string fname);
        Task clientler(List<Client> clients);
        Task ReceiveMessageHistory(List<string> list);
    }
}
