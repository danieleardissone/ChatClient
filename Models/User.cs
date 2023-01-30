namespace ChatClient.Models
{
    public class User
    {
        public string ChatName { get; set; }

        public User(string chatName)
        {
            ChatName = chatName;
        }
    }
}
