using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace API_WebApplication1.Models
{
    public class CustomerDTO
    {
     

        public long ID { get; set; }
        public string CIF_NUMBER { get; set; } = string.Empty;
        public string CUSTOMER_TYPE { get; set; } = string.Empty; //add in later
        public string CUSTOMER_NAME { get; set; } = string.Empty; //add in later
        public DateOnly DATA_DATE { get; set; } 

    }


}
