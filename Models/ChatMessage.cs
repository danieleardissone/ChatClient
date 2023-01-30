using ChatClient.Consts;
using ChatClient.Enums;

namespace ChatClient.Models
{
    public class ChatMessage
    {
        public string Data { get; set; }
        public User? Sender { get; set; }

        public ChatMessageTypeEnum Type { get; set; }

        public ChatMessage(ChatMessageTypeEnum type, User sender, string data)
        {
            Type = type;
            Sender = sender;

            switch (Type)
            {
                case ChatMessageTypeEnum.CONNECTION:
                    {
                        Data = ChatMessagePrefix.CONNECTION + (sender?.ChatName ?? string.Empty)  + "|" + data;
                        break;
                    }

                default:
                case ChatMessageTypeEnum.MESSAGE:
                    {
                        Data = ChatMessagePrefix.MESSAGE + data;
                        break;
                    }
            }
        }
    }
}
