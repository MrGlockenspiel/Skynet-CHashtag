using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Reflection;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;
using OpenAI;

public class MyFirstModule : BaseCommandModule {
    [Command("gpt3")]
    public async Task GPT3Command(CommandContext ctx, string prompt) {
        var api = new OpenAIAPI(apiKeys:"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");
        var request = new CompletionRequestBuilder()
            .WithPrompt($"{prompt}")
            .WithMaxTokens(100)
            .Build();
        
        var result = await api.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }
    
    [Command("greentext")]
    public async Task GreentextCommand(CommandContext ctx, string prompt) {
        var api = new OpenAIAPI(apiKeys:"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");
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a 4chan greentext \n{prompt}")
            .WithMaxTokens(200)
            .Build();
        
        var result = await api.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }

    [Command("time")]
    public async Task TimetestCommand(CommandContext ctx) {
        string time = DateTime.Now.ToString("h:mm");
        await ctx.RespondAsync($"{time}");
    }

    [Command("tts")]
    public async Task TTSCommand(CommandContext ctx, string text) {
        using var client = new HttpClient();
        string content = "";
        text = text.Replace("+", "plus").Replace(" ", "+").Replace("&", "and");
        
        var response = await client.GetAsync($"https://api16-normal-useast5.us.tiktokv.com/media/api/text/speech/invoke/?text_speaker=en_us_002&req_text={text}");
        if (response.IsSuccessStatusCode) {
            content = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(content);
            content = data.data.v_str;
        }
        else {
            content = "failed";
        }

        Console.WriteLine(content);
        byte[] b64Data = Convert.FromBase64String(content);
        string decodedContent = Encoding.UTF8.GetString(b64Data);
        await File.WriteAllTextAsync("voice.mp3", decodedContent);
        
        using (var fs = new FileStream("voice.mp3", FileMode.Open, FileAccess.Read))
        {
            var msg = await new DiscordMessageBuilder()
                .WithContent("the")
                .WithFiles(new Dictionary<string, Stream>() { { "voice1.mp3", fs } })
                .SendAsync(ctx.Channel);           
        }
    }
}

namespace Skynet_CHashtag {
    class Program {
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
            
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            await TimeCheck("01:00", TimeSpan.FromSeconds(30), CancellationToken.None, discord);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static async Task TimeCheck(String time, TimeSpan interval, CancellationToken cancellationToken, DiscordClient s) {
            while (true) {
                await Task.Delay(interval, cancellationToken);
                string currentTime = DateTime.Now.ToString("HH:mm");
                Console.WriteLine("iterating to {0}, getting to {1}", currentTime, time);
                if (currentTime == time) {
                    Console.WriteLine("calling");
                    await WhoUpPlayingWithTheyWorm(s);
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
                    var msg = await new DiscordMessageBuilder()
                        .WithContent("@everyone who up playing with they worm?")
                        .WithFiles(new Dictionary<string, Stream>() { { "worm.png", fs } })
                        .SendAsync(channel);
                }
            }
        }
    }
}