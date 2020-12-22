using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Info
    {
        private static Info info;

        public const ulong BotOwner = 587018784360103966;
        public static ulong ClientID => info.clientId;
        public static string Token => info.token;

        public ulong clientId = 0;
        public string token = "token";

        public static async Task LoadAtPath(string pathToInfo)
        {
            info = new();
            if (!string.IsNullOrEmpty(pathToInfo))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = true;
                if (File.Exists(pathToInfo))
                {
                    try
                    {
                        using FileStream openStream = File.OpenRead(pathToInfo);
                        info = await JsonSerializer.DeserializeAsync<Info>(openStream, options);
                        return;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }
                }

                string json = JsonSerializer.Serialize(info, options);
                File.WriteAllText(pathToInfo, json);
            }
        }
    }
}
