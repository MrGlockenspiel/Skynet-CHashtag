using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class Greentext : BaseCommandModule
{
    [Command("greentext")]
    public static async Task GreentextCommand(CommandContext ctx, string prompt) {
        var api = new OpenAIAPI(apiKeys:"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");
        var request = new CompletionRequestBuilder()
            .WithPrompt($"write me a 4chan greentext \n{prompt}")
            .WithMaxTokens(200)
            .Build();
        
        var result = await api.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }
}