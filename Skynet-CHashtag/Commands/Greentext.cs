using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class GreentextCommand : ApplicationCommandModule {
    [SlashCommand("greentext", "Generate an AI 4chan greentext")]
    public async Task GreentextSlashCommand(InteractionContext ctx, [Option("prompt", "Prompt for generation")] string prompt, [Option("temperature", "Generation temperature (0-1, 0.8 is default)")] double temperature = 0.8) {
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        switch (temperature)
        {
            case < 0:
                temperature = 0.0;
                break;
            case > 1:
                temperature = 1.0;
                break;
        }
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a story in the form of a 4chan greentext. starting with \">be me\" and every line after starting with \">\". the topic will be {prompt}")
            .WithMaxTokens(200)
            .WithTemperature(temperature)
            .Build();
        
        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        string filteredResult = Regex.Replace($"{result}", "\u006E\u0069\u0067\u0067", "fell", RegexOptions.IgnoreCase); // racism filter
        await ctx.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"```\n{filteredResult}\n```"));
    }
}