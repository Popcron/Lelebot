﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Info
    {
        public ulong ClientID { get; set; } = 0;
        public string Token { get; set; } = "token";

        public static async Task<Info> LoadAtPath(string pathToInfo)
        {
            Info info = new();
            if (File.Exists(pathToInfo))
            {
                try
                {
                    using FileStream openStream = File.OpenRead(pathToInfo);
                    info = await JsonSerializer.DeserializeAsync<Info>(openStream);
                    return info;
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return info;
                }
            }
            else
            {
                string json = JsonSerializer.Serialize(info);
                File.WriteAllText(pathToInfo, json);
            }

            return info;
        }
    }
}
