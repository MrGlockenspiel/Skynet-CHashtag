using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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