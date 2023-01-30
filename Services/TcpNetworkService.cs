using ChatClient.Enums;
using ChatClient.Exceptions;
using ChatClient.Models;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Services
{
    public class TcpNetworkService : ITcpNetworkService
    {
        TcpClient _client;
        public TcpClient Client { get => _client; set => _client = value; }
        
        NetworkStream _serverStream;
        public NetworkStream ServerStream { get => _serverStream; set => _serverStream = value; }

        #region Connection management
        public TcpClient OpenConnection(string host, int port)
        {
            if (_client != null)
                throw new TcpConnectionException("Connection is already open.");
            else
            {
                try
                {
                    _client = new TcpClient();
                    _client.Connect(host, port);
                }
                catch (Exception)
                {
                    _client = null;
                    throw new TcpConnectionException("Connection cannot be established.");
                }
            }

            return _client;
        }

        public bool CloseConnection()
        {
            if (_client == null)
                throw new TcpConnectionException("Connection is not open or already closed.");

            try
            {
                _client.Close();
            }
            catch (Exception)
            {
                throw new TcpConnectionException("Connection cannot be closed.");
            }
            finally
            {
                _client = null;
            }

            return true;
        }
        #endregion

        #region Server interactions
        public void SendMessage(ChatMessageTypeEnum messageType, User currentUser, string data)
        {
            if (_client == null)
            {
                throw new TcpConnectionException("Connection is not open or closed.");
            }

            ChatMessage msgToSend = new ChatMessage(messageType, currentUser, data);

            // Send
            ServerStream = _client.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(msgToSend.Data);
            ServerStream.Write(outStream, 0, outStream.Length);
            ServerStream.Flush();
        }

        public string GetMessage(object obj)
        {
            if (_client != null && _client.Connected)
            {
                try
                {
                    byte[] inStream = new byte[_client.ReceiveBufferSize];
                    int bytesRead = ServerStream.Read(inStream, 0, _client.ReceiveBufferSize);
                    return Encoding.ASCII.GetString(inStream, 0, bytesRead);
                }
                catch (IOException) { }
            }

            return string.Empty;
        }
        #endregion
    }
}
