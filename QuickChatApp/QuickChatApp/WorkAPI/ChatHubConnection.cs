using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

public class ChatHubConnection
{
    private readonly HubConnection _hubConnection;
    private readonly List<string> _joinedGroups = new List<string>();
    public event Action<string, string> ReceiveMessage;

    public ChatHubConnection(string url)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            ReceiveMessage?.Invoke(user, message);
        });
    }

    public async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(string user, string message)
    {
        try
        {
            await _hubConnection.InvokeAsync("SendMessage", user, message);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Send failed: {ex.Message}");
        }
    }

    public async Task JoinGroupAsync(string groupName)
    {
        try
        {
            await _hubConnection.InvokeAsync("JoinGroup", groupName);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Join group failed: {ex.Message}");
        }
    }
    public async Task JoinAllUserChatsAsync(IEnumerable<int> chats)
    {
        // Подключаемся к каждому чату
        foreach (int groupName in chats)
        {
            if (!_joinedGroups.Contains(groupName.ToString()))
            {
                await _hubConnection.InvokeAsync("JoinGroup", groupName);
                _joinedGroups.Add(groupName.ToString());
            }
        }
    }
    public async Task SendMessageToGroupAsync(string groupName, string user, string message)
    {
        try
        {
            await _hubConnection.InvokeAsync("SendMessageToGroup", groupName, user, message);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Send to group failed: {ex.Message}");
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
        }
    }
}
