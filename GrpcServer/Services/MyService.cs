using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class MyService : GRPCservice.GRPCserviceBase
    {
        List<send_data> cust = new List<send_data>
            {
                new send_data
                {
                    Id =1,
                    Name ="aprajita",
                    Height =2.5f,
                    IsMale =false
                },
                 new send_data
                {
                    Id =3,
                    Name ="goly",
                    Height =3.5f,
                    IsMale =true

                },
                new send_data
                {
                    Id =4,
                    Name ="sad",
                    Height =1.5f,
                    IsMale =true

                },

            };

      //  private static string tempString = "";
       // public static List<send_data> storage;

        private readonly ILogger<MyService> _logger;
        public MyService(ILogger<MyService> logger)
        {
            _logger = logger;
        }
        public override Task<read_data> send(send_data request, ServerCallContext context)
        {
            Console.WriteLine();
            cust.Add(new send_data { Id = request.Id, Name = request.Name,Height=request.Height,IsMale =request.IsMale });
            foreach (var m in cust)
            {
                Console.WriteLine("Name " + m.Name);
            }
            return Task.FromResult(new read_data
            {
                DataReply = "Added data :" + "Name : " + request.Name + " Id " + request.Id.ToString() +" Height " + request.Height.ToString() + " IsMale "+ request.IsMale.ToString()
            });
        }
        public override async Task first(global::NewCustomerRequest request, IServerStreamWriter<send_data> responseStream, ServerCallContext context)
        {
            
            foreach (var cu in cust)
            {
                await Task.Delay(5000);
                await responseStream.WriteAsync(cu);
            }
        }


        public override async Task DownloadImage(Empty request, IServerStreamWriter<DownloadImageResponse> responseStream, ServerCallContext context)
        {
            byte[] data = File.ReadAllBytes("C:\\Users\\golur\\source\\repos\\GrpcDemo\\GrpcServer\\download.jfif");

            int parts = data.Length / 100;
            byte[] part;
            _logger.LogInformation("Sending image chunk...");
            for (int i = 0; i < 100; i++)
            {
                part = new byte[parts];
                Array.Copy(data, i * parts, part, 0, parts);

                await responseStream.WriteAsync(new DownloadImageResponse
                {
                    Image = Google.Protobuf.ByteString.CopyFrom(part)
                });
            }
            _logger.LogInformation("Image was sent.");
        }
    }
}