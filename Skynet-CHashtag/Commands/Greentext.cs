using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class Greentext : BaseCommandModule {
    [Command("greentext")]
    public async Task GreentextCommand(CommandContext ctx, [RemainingText] string prompt) {
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a 4chan greentext \n{prompt}")
            .WithMaxTokens(200)
            .Build();
        
        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"```\n{result}\n```");
    }
}

public class GreentextSlash : ApplicationCommandModule {
    [SlashCommand("greentext", "Generate an AI 4chan greentext")]
    public async Task GreentextSlashCommand(InteractionContext ctx, [Option("prompt", "Prompt for generation")] string prompt) {
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a 4chan greentext \n{prompt}")
            .WithMaxTokens(200)
            .Build();
        
        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        
        await ctx.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"```\n{result}\n```"));
    }
}