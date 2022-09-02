using System.Reflection;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;
using OpenAI;


namespace Skynet_CHashtag {
    class SkynetCHashtag {
        static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync() {
            var discord = new DiscordClient(new DiscordConfiguration() {
                Token = "OTMzMDk3MTA3OTkxMTkxNjMz.YeckZg.OJqwyibLyZkIDVROxxs-yAbVvkQ",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration() {
                StringPrefixes = new[] { "!" }
            });
            
            commands.RegisterCommands(typeof(SkynetCHashtag).Assembly);

            await TimeCheck("01:00", TimeSpan.FromSeconds(30), CancellationToken.None, discord);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static async Task TimeCheck(String time, TimeSpan interval, CancellationToken cancellationToken, DiscordClient s) {
            while (true) {
                await Task.Delay(interval, cancellationToken);
                string currentTime = DateTime.Now.ToString("HH:mm");
                //Console.WriteLine("iterating to {0}, getting to {1}", currentTime, time); // debug
                if (currentTime == time) {
                    //Console.WriteLine("calling"); // debug
                    await WhoUpPlayingWithTheyWorm(s);
                }

                if (cancellationToken.IsCancellationRequested) {
                    return;
                }
            }
        }

        static async Task WhoUpPlayingWithTheyWorm(DiscordClient s) {
            DiscordChannel channel = await s.GetChannelAsync(916080447765770291); // #announcements, IT server
            //DiscordChannel channel = await s.GetChannelAsync(915592979039789106); // #bot-sandbox, IT server
            Random rnd = new Random();
            int chance  = rnd.Next(1, 7);
            if (chance == 6) {
                await using (var fs = new FileStream("../../../../worm.png", FileMode.Open, FileAccess.Read)) {
                    await new DiscordMessageBuilder()
                        .WithContent("@everyone who up playing with they worm?")
                        .WithFiles(new Dictionary<string, Stream>() { { "worm.png", fs } })
                        .SendAsync(channel);
                }
            }
        }
    }
}