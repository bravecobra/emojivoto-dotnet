using System;
using System.Text;
using System.Threading.Tasks;
using Emojivoto.V1;
using Grpc.Net.Client;

namespace EmojiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmojiService.EmojiServiceClient(channel);
            var reply = await client.ListAllAsync(
                 new ListAllEmojiRequest());
            foreach (var emoji in reply.List)
            {
                Console.WriteLine($"{emoji.Shortcode} => {emoji.Unicode}");
            }
            // Console.WriteLine("Press any key to exit...");
            // Console.ReadKey();
        }
    }
}
