using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Info
    {
        public ulong ClientID { get; private set; } = 0;
        public string Token { get; private set; } = "token";

        private Info()
        {

        }

        public static async Task<Info> LoadFromFile()
        {
            string pathToDirectory = Program.PathToDirectory;
            string pathToInfo = Path.Combine(pathToDirectory, "info.json");
            if (File.Exists(pathToInfo))
            {
                using FileStream openStream = File.OpenRead(pathToInfo);
                return await JsonSerializer.DeserializeAsync<Info>(openStream);
            }
            else
            {
                string json = JsonSerializer.Serialize(new Info());
                File.WriteAllText(pathToInfo, json);
            }

            return new Info();
        }
    }
}
