using Microsoft.AspNetCore.SignalR;

namespace WebApplication2
{

    public class ChatHub : Hub
    {
        public async Task SendMessageToChat(int chatId, string user, string message)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", user, message);
        }

        // Пример метода для присоединения к группе
        public async Task JoinGroup(string groupName)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", "Server", $"{Context.UserIdentifier} joined {groupName}");
        }

        // Пример метода для отправки сообщений в группу
        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }
    }
}
