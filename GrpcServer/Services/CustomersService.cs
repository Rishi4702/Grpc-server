using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {

        public override Task<out_i> addInt(inp req, ServerCallContext ctx) {
            string comment ="some  comment";
            int result =req.Num1+req.Num2;
            return Task.FromResult(new out_i {Reply = result,Comment =comment });
        
        }
        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.Firstname = "Rishav";
                output.Lastname = "Sagar";
            }
            else if (request.UserId == 2)
            {
                output.Firstname = "Golu";
                output.Lastname = "beti";
            }
            else {
                output.Firstname = "Chuitya";
                output.Lastname = "Sala";
            }

            return Task.FromResult(output);
        }
        public override async Task GetNewCustomers(NewCustomerRequest request, 
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> cust = new List<CustomerModel>
            {
                new CustomerModel
                {
                    Firstname ="aprajita",
                    Lastname="Sharma",
                    EmailAddress="Appy@gmail.com",
                    Age = 21,
                    IsAlive =false
                },
                new CustomerModel
                {
                    Firstname ="ania",
                    Lastname="kulas",
                    EmailAddress="ania@gmail.com",
                    Age = 11,
                    IsAlive =true
                },
                new CustomerModel
                {
                    Firstname ="vikash",
                    Lastname="kumar",
                    EmailAddress="vk@gmail.com",
                    Age = 31,
                    IsAlive =true
                }

            };
            foreach (var cu in cust) {
                await Task.Delay(5000);
                await  responseStream.WriteAsync(cu);
            }
        }
    }
}
