using Emojivoto.V1;
using Grpc.Net.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EmojiClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmojiService.EmojiServiceClient(channel);
            var reply = await client.ListAllAsync(new ListAllEmojiRequest());
            foreach (var emoji in reply.List)
            {
                Console.WriteLine($"{emoji.Shortcode} => {emoji.Unicode}");
            }

            var result = await client.FindByShortcodeAsync(new FindByShortcodeRequest() { Shortcode = ":thumbsup:" });
            Console.WriteLine($"\nAll seems well {result.Emoji.Unicode}");
        }
    }
}
