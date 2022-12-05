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
        
            OpenAi = new OpenAIAPI(apiKeys:openAiKey, "text-davinci-003");
            
            var discord = new DiscordClient(new DiscordConfiguration {
                Token = discordToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            var slashCommands = discord.UseSlashCommands();
            slashCommands.RegisterCommands(typeof(SkynetCHashtag).Assembly);

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}