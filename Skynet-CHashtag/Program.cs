using System;
using System.Collections.Generic;
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
            
            /*discord.MessageCreated += async (s, e) => {
                if (e.Message.Content.ToLower().StartsWith("ligma")) {
                    await e.Message.RespondAsync("whats " + e.Message.Content);
                }
            };*/
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}