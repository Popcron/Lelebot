using Discord.WebSocket;

namespace Lelebot
{
    public class Context
    {
        public SocketUser Author { get; set; }
        public ISocketMessageChannel Channel { get; set; }
        public SocketMessage Message { get; set; }
        public SocketGuild Guild { get; set; }

        /// <summary>
        /// Raw text from the message.
        /// </summary>
        public string Text { get; set; }

        public string[] Args { get; set; }
        public string Command { get; set; }

        public async void DeleteMessage()
        {
            if (Message != null)
            {
                try
                {
                    await Message.DeleteAsync();
                }
                catch
                {

                }
            }
        }
    }
}
