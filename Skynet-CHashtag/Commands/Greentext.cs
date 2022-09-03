using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class Greentext : BaseCommandModule
{
    [Command("greentext")]
    public async Task GreentextCommand(CommandContext ctx, string prompt) {
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a 4chan greentext \n{prompt}")
            .WithMaxTokens(200)
            .Build();
        
        var result = await SkynetCHashtag.OpenAi.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }
}