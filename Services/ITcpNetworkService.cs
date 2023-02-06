using ChatClient.Enums;
using ChatClient.Models;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Services
{
    public interface ITcpNetworkService
    {
        TcpClient Client { get; }
        TcpClient OpenConnection(string host, int port);
        bool CloseConnection();
        void SendMessage(ChatMessageTypeEnum messageType, User currentUser, string data);
        string GetMessage(object obj);
    }
}
