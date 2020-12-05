﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Info
    {
        public ulong ClientID { get; private set; } = 0;
        public string Token { get; private set; } = "token";

        public static async Task<Info> LoadAtPath(string pathToInfo)
        {
            Info info = new();
            if (File.Exists(pathToInfo))
            {
                try
                {
                    using FileStream openStream = File.OpenRead(pathToInfo);
                    return await JsonSerializer.DeserializeAsync<Info>(openStream);
                }
                catch (Exception e)
                {
                    string fileName = Path.GetFileName(pathToInfo);
                    Console.WriteLine($"Error while reading {fileName}");
                    Console.WriteLine(e.Message);
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
