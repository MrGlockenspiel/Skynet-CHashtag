using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI;
using Tomlyn;

namespace Skynet_CHashtag {
    public class SkynetCHashtag {

        internal static OpenAIAPI OpenAi;
        public static string openAiKey;
        public static string discordToken;
        
        private static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync() {
            string userHome = Environment.GetEnvironmentVariable("HOME");
            string configPath = userHome + "/skynet-config.toml";
            
            if (!File.Exists(configPath)) {
                string createText = "discordToken = <put key here> \nopenAiKey = <put key here>" + Environment.NewLine;
                File.WriteAllText(configPath, createText);
                Console.WriteLine("No config file found, generating template at " + configPath + " and closing");
                Environment.Exit(0);
            }
            
            string configToml = File.ReadAllText(configPath);
            var model = Toml.ToModel(configToml);
            
            discordToken = (string)model["discordToken"]!;
            openAiKey = (string)model["openAiKey"]!;
        
            OpenAi = new OpenAIAPI(apiKeys:openAiKey);
            
            var discord = new DiscordClient(new DiscordConfiguration {
                Token = discordToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var slashCommands = discord.UseSlashCommands();
            slashCommands.RegisterCommands(typeof(SkynetCHashtag).Assembly);

            TimeCheck("01:00", TimeSpan.FromSeconds(30), discord);

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        // ignore all of this its not real
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
            DiscordChannel channel = await s.GetChannelAsync(916080447765770291); // #announcements channel in the boys' server
            Random rnd = new Random();
            int chance = rnd.Next(1, 10);
            if (chance == 6) {
                await using var wormFile = new FileStream($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}/worm.png", FileMode.Open, FileAccess.Read);
                await new DiscordMessageBuilder()
                    .WithContent("@everyone who up playing with they worm?")
                    .WithFiles(new Dictionary<string, Stream>() { { "worm.png", wormFile } })
                    .WithAllowedMention(new EveryoneMention())
                    .SendAsync(channel);
            }
        }
    }
}