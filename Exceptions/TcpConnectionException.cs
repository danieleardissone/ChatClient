namespace ChatClient.Exceptions
{
    public class TcpConnectionException : Exception
    {
        public TcpConnectionException()
        {
        }

        public TcpConnectionException(string message)
            : base(message)
        {
        }

        public TcpConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
