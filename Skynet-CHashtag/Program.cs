using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI;

namespace Skynet_CHashtag {
    internal class SkynetCHashtag {
        
        internal static readonly OpenAIAPI OpenAi = new OpenAIAPI(apiKeys:"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");
        
        private static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync() {
            var discord = new DiscordClient(new DiscordConfiguration {
                Token = "OTMzMDk3MTA3OTkxMTkxNjMz.YeckZg.OJqwyibLyZkIDVROxxs-yAbVvkQ",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                StringPrefixes = new[] { "!" },
                EnableDefaultHelp = false
            });
            
            var slash = discord.UseSlashCommands();

            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            TimeCheck("01:00", TimeSpan.FromSeconds(30), discord);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task TimeCheck(string time, TimeSpan interval, DiscordClient s) {
            while (true) {
                await Task.Delay(interval);
                string currentTime = DateTime.Now.ToString("HH:mm");
                // Console.WriteLine($"iterating to {currentTime}, getting to {time}"); // debug
                if (currentTime == time) {
                    // Console.WriteLine("calling"); // debug
                    await WhoUpPlayingWithTheyWorm(s);
                }
            }
        }

        private static async Task WhoUpPlayingWithTheyWorm(DiscordClient s) {
            DiscordChannel channel = await s.GetChannelAsync(916080447765770291); // #announcements, IT server
            // DiscordChannel channel = await s.GetChannelAsync(915592979039789106); // #bot-sandbox, IT server
            Random rnd = new Random();
            int chance = rnd.Next(1, 7);
            if (chance == 6)
            {
                await using var wormFile = new FileStream("../../../../worm.png", FileMode.Open, FileAccess.Read);
                await new DiscordMessageBuilder()
                    .WithContent("@everyone who up playing with they worm?")
                    .WithFiles(new Dictionary<string, Stream>() { { "worm.png", wormFile } })
                    .SendAsync(channel);
            }
        }
    }
}