using Discord;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RoleRainbow : Processor
    {
        public class Config
        {
            public ulong guildId = 0;
            public ulong roleId = 0;
            public float interval = 1f;
            public int maxSteps = 54;
        }

        private Config config;
        private IGuild myGuild;
        private IRole role;
        private int step;

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

                int hue = (int)(step / (float)config.maxSteps * 360f);
                ColorRGB rgb = new ColorHSV(hue, 1, 1).GetRGB();
                Color color = new Color(rgb.R, rgb.G, rgb.B);
                await Task.Delay(ms);
                role.ModifyAsync(x => x.Color = color);

                step++;
                if (step >= config.maxSteps)
                {
                    step = 0;
                }
            }
        }
    }
}
