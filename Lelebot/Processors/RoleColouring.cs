﻿using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RoleColouring : Processor
    {
        public class Config
        {
            public ulong guildId = 0;
            public ulong roleId = 0;
            public float interval = 1f;
            public string[] colors = { "#fcba03" };
        }

        private Config config;
        private IGuild myGuild;
        private IRole role;
        private int colorIndex;
        private Color[] colors;

        public (int r, int g, int b) ToRGB(string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            if (hexColor.Length != 6)
            {
                throw new Exception("Color not valid");
            }

            int r = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            return (r, g, b);
        }

        public override async void OnCreated(Bot bot)
        {
            EnsureTemplateExists();
            config = LoadConfig();

            myGuild = bot.Client.GetGuild(config.guildId);
            if (myGuild != null)
            {
                //check if permissions are available
                IGuildUser guildUser = await myGuild.GetUserAsync(bot.Client.CurrentUser.Id);
                if (!guildUser.GuildPermissions.ManageRoles)
                {
                    Console.WriteLine($"[role crayons] cant start, because this bot doesnt permission to manage roles");
                    return;
                }

                //find the target guild user
                role = myGuild.GetRole(config.roleId);
                if (role == null)
                {
                    Console.WriteLine($"[role crayons] cant start because the role with id {config.roleId} was not found");
                    return;
                }
                else
                {
                    Console.WriteLine($"[role crayons] role selected is {role.Name}");
                }

                if (config.colors.Length == 0)
                {
                    Console.WriteLine($"[role crayons] no colors are set");
                    return;
                }

                colors = new Color[config.colors.Length];
                for (int i = 0; i < config.colors.Length; i++)
                {
                    (int r, int g, int b) = ToRGB(config.colors[i]);
                    colors[i] = new Color(r, g, b);
                }

                Console.WriteLine($"[role crayons] starting the role colouring machine");
                ColorGoVroom();
            }
            else
            {
                Console.WriteLine($"[role crayons] couldnt find server with id {config.guildId} was not found");
            }
        }

        private Config LoadConfig()
        {
            string pathToFile = GetLocalFilePath("config.json");
            if (File.Exists(pathToFile))
            {
                try
                {
                    string json = File.ReadAllText(pathToFile);
                    return JsonConvert.DeserializeObject<Config>(json);
                }
                catch
                {
                    return new Config();
                }
            }
            else
            {
                return new Config();
            }
        }

        private void EnsureTemplateExists()
        {
            string pathToTemplate = GetLocalFilePath("config.json.template");
            Config template = new Config();
            string json = JsonConvert.SerializeObject(template, Formatting.Indented);
            File.WriteAllText(pathToTemplate, json);
        }

        private async void ColorGoVroom()
        {
            while (true)
            {
                int ms = (int)(1000f * config.interval);
                if (ms < 100)
                {
                    ms = 100;
                }

                await Task.Delay(ms);
                await role.ModifyAsync(x => x.Color = colors[colorIndex]);

                colorIndex++;
                if (colorIndex >= config.colors.Length)
                {
                    colorIndex = 0;
                }
            }
        }
    }
}
