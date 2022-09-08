using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class Gpt3 : BaseCommandModule {
    [Command("gpt3")]
    public async Task Gpt3Command(CommandContext ctx, [RemainingText] string prompt) {
        var request = new CompletionRequestBuilder()
            .WithPrompt($"{prompt}")
            .WithMaxTokens(100)
            .Build();
        
        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }
}

public class Gpt3Slash : ApplicationCommandModule {
    [SlashCommand("gpt3", "Generate a response with GPT3")]
    public async Task Gpt3SlashCommand(InteractionContext ctx, [Option("prompt", "Prompt for generation")] string prompt) {
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        var request = new CompletionRequestBuilder()
            .WithPrompt($"{prompt}")
            .WithMaxTokens(100)
            .Build();

        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        
        await ctx.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"{result}"));
    }
}