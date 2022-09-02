using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using OpenAI;

namespace Skynet_CHashtag.Commands;

public class Gpt3 : BaseCommandModule
{
    [Command("gpt3")]
    public static async Task Gpt3Command(CommandContext ctx, string prompt) {
        var api = new OpenAIAPI(apiKeys:"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");
        var request = new CompletionRequestBuilder()
            .WithPrompt($"{prompt}")
            .WithMaxTokens(100)
            .Build();
        
        var result = await api.Completions.CreateCompletionAsync(request);
        await ctx.RespondAsync($"{result}");
    }
}