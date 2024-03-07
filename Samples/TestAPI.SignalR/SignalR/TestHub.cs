using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace TestAPI.SignalR.SignalR
{
    public class TestHub:Hub
    {
        public async Task SendMessageToAll(object testObject)
        { 
            await Clients.Others.SendAsync("postnewmessage", JsonSerializer.Serialize(testObject));
        }
    }
}
