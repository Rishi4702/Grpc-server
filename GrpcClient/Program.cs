using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace gRPCclient
{
    class Program
    {
        static async void downImag(GRPCservice.GRPCserviceClient gd) {
            // Image download using stream
            Console.WriteLine("Starting downloading image");
            var downloadedImage = gd.DownloadImage(new Empty());
            List<byte> array = new List<byte>();
            int j = 0;
            await foreach (var imagePart in downloadedImage.ResponseStream.ReadAllAsync())
            {
                for (int i = 0; i < imagePart.Image.Length; i++)
                {
                    array.Add(imagePart.Image[i]);
                }
                Console.Write("\r[{0}] {1}%", "*".PadLeft(j / 10, '*').PadRight(9, ' '), ++j);
                Thread.Sleep(20);
            }
            File.WriteAllBytes("../../../fromServer.jpg", array.ToArray());
            Console.WriteLine();
            Console.WriteLine("Finsihed downloading image");

        }
        static async void showClient(GRPCservice.GRPCserviceClient gd) {
           
            using (var call = gd.first(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currrentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currrentCustomer.Id} {currrentCustomer.Name} {currrentCustomer.Height} {currrentCustomer.IsMale}");
                }
            }
        }

        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GRPCservice.GRPCserviceClient(channel);
            Console.WriteLine("Client was started");
            //sending data to server unary
            //async DownloD
            downImag(client);
            //asynchronus
            showClient(client);
            //synchronus
            var reply = client.send(new send_data
            {
                Id = 2,
                Name = "Rishav",
                Height = 2.4f,
                IsMale =true
            }
            ) ;
            Console.WriteLine("\n"+reply.DataReply);

            //
            channel.ShutdownAsync().Wait();
            Console.Read();
        }
    }
}