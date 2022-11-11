using System.Net.Http.Headers;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;

namespace Skynet_CHashtag.Commands; 

// like 70% of this code is from https://betterprogramming.pub/create-a-text-to-image-generator-using-dall-e-api-in-c-net-7df58d940e79

public class input {
    public string? prompt { get; set; }
    public short? n { get; set; }
    public string? size { get; set; }
}

public class ImageLink {
    public string? url { get; set; }
}

public class ResponseModel {
    public long created { get; set; }
    public List<ImageLink>? data { get; set; }
}

public class GenImageCommand : ApplicationCommandModule {
    [SlashCommand("dalle2", "Generate an image with DALL-E")]
    public async Task GenImageSlashCommand(InteractionContext ctx,
        [Option("prompt", "Prompt for generation")] string prompt) {
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        var apiInput = new input();
        apiInput.prompt = prompt;
        apiInput.n = 1;
        apiInput.size = "1024x1024";
        
        var resp = new ResponseModel();
        using (var client = new HttpClient()) {
            client.DefaultRequestHeaders.Clear();
            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SkynetCHashtag.openAiKey);
            
            var message = await client.PostAsync("https://api.openai.com/v1/images/generations",
                new StringContent(JsonConvert.SerializeObject(apiInput), Encoding.UTF8, "application/json"));
            
            if (message.IsSuccessStatusCode) {
                var content = await message.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ResponseModel>(content);
            }
        }

        foreach (var link in resp.data) {
            await ctx.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().WithContent($"{link.url}"));
        }
    }
}