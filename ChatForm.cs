using System.Net.Sockets;
using System.Text;
using ChatClient.Enums;
using ChatClient.Exceptions;
using ChatClient.Models;
using ChatClient.Services;

namespace ChatClient
{
    public partial class ChatForm : Form
    {
        #region Constants
        private string HOST = "127.0.0.1";
        private int PORT = 10000;
        #endregion

        #region Private properties
        ITcpNetworkService _tcpNetworkSvc;
        User? _currentUser;
        string? _readData;
        System.Threading.Timer? _timerThread;
        #endregion

        public ChatForm(ITcpNetworkService tcpNetworkService)
        {
            InitializeComponent();

            _tcpNetworkSvc = tcpNetworkService;
        }

        #region Thread management
        private void CreateThread()
        {
            TimerCallback msgCallback = GetMessage;
            _timerThread = new System.Threading.Timer(msgCallback, "msg", 500, 500);
        }

        private void DisposeThread()
        {
            _timerThread?.Dispose();
        }
        #endregion

        #region Connection management
        private void CloseConnection()
        {
            try
            {
                if (_tcpNetworkSvc.CloseConnection())
                {
                    PrintMsg("Connection closed successfully.");

                    txtChatName.Clear();
                    DisposeThread();
                    _currentUser = null;
                }
            }
            catch (TcpConnectionException tcpEx)
            {
                PrintMsg(tcpEx.Message);
                return;
            }
        }
        #endregion

        #region Server interactions
        private void GetMessage(object obj)
        {
            PrintMsg("" + _tcpNetworkSvc.GetMessage(obj));
        }
        #endregion

        #region Print messages
        private void PrintMsg(string msg)
        {
            _readData = msg;
            Msg();
        }

        private void Msg()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Msg));
            else if (!String.IsNullOrEmpty(_readData))
                txtChat.AppendText(Environment.NewLine + " >> " + _readData);
        }
        #endregion

        #region Control events
        #region Button click
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtChatName.Text))
            {
                PrintMsg("Please choose a chat name.");
                return;
            }

            try
            {
                _tcpNetworkSvc.OpenConnection(HOST, PORT);
                PrintMsg("Connected to Chat Server ...");
            }
            catch (TcpConnectionException tcpEx)
            {
                PrintMsg(tcpEx.Message);
                return;
            }

            if (_tcpNetworkSvc.Client != null)
                _currentUser = new User(txtChatName.Text);

            try
            {
                _tcpNetworkSvc.SendMessage(ChatMessageTypeEnum.CONNECTION, _currentUser, txtChatName.Text);
            }
            catch (TcpConnectionException tcpEx)
            {
                PrintMsg(tcpEx.Message);
                return;
            }

            CreateThread();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            _tcpNetworkSvc.SendMessage(ChatMessageTypeEnum.MESSAGE, _currentUser, txtMessage.Text);
            txtMessage.Clear();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }
        #endregion

        #region KeyPress
        private void txtChatName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                btnConnect_Click(sender, e);
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                btnSendMessage_Click(sender, e);
        }
        #endregion

        #region Other
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }
        #endregion
        #endregion
    }
}