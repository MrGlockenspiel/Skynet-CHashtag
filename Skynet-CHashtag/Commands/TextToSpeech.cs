using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;

namespace Skynet_CHashtag.Commands;

public class TextToSpeech : BaseCommandModule
{
    [Command("tts")]
    [RequireBotPermissions(Permissions.AttachFiles)]
    public async Task TextToSpeechCommand(CommandContext ctx, string text) {
        using var client = new HttpClient();
        string content;
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

        await using var voiceFile = new FileStream("voice.mp3", FileMode.Open, FileAccess.Read);
        await new DiscordMessageBuilder()
            .WithContent("the")
            .WithFiles(new Dictionary<string, Stream> { { "voice1.mp3", voiceFile } })
            .SendAsync(ctx.Channel);
    }
}