using System.Text;

namespace Lelebot
{
    public class Message
    {
        private StringBuilder textBuilder = new StringBuilder();

        public string Text => textBuilder.ToString();
        public StringBuilder StringBuilder => textBuilder;

        public void Append(object obj = null) => textBuilder.Append(obj?.ToString() ?? "");
        public void AppendLine(object obj = null) => textBuilder.AppendLine(obj?.ToString() ?? "");
    }
}