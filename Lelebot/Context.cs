using Discord.WebSocket;

namespace Lelebot
{
    public class Context
    {
        public SocketUser Author { get; set; }
        public ISocketMessageChannel Channel { get; set; }
        public string Text { get; set; }
        public string[] Args { get; set; }
        public string Command { get; set; }
    }
}
