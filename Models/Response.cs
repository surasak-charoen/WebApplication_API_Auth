using API_WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_WebApplication1.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public List<Customer> Customers { get; set; } // Updated to handle a list of Customers
        public List<Merchant> Merchants { get; internal set; }

        //public Customer Customer { get; set; } // Keep this if you still need single Customer retrieval
        public Response()
        {
            Customers = new List<Customer>();
        }

    }

}

