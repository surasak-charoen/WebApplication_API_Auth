using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace API_WebApplication1.Models
{
    public class Customer
    {
     
        public long ID { get; set; }
        public string CIF_NUMBER { get; set; } = string.Empty;
        public long ID_TYPE { get; set; }
        public string ID_NUMBER { get; set; } = string.Empty;
        public string ID_COUNTRY { get; set; } = string.Empty;
        public DateOnly DATA_DATE { get; set; } 

        public string CUSTOMER_TYPE { get; set; } = string.Empty; //add in later
        public string CUSTOMER_NAME { get; set; } = string.Empty; //add in later
        public DateTime CREATE_DATETIME { get; set; } //add in later
        public string CUS_TITLE { get; set; } = string.Empty;  //add in later
        public string CUS_FIRST { get; set; } = string.Empty; //add in later
        public string CUS_LAST { get; set; } = string.Empty; //add in later
        public string NEXT_ANN_REV_DATE { get; set; } = string.Empty; //add in later*/

        public string CUS_NAME { get; set; } = string.Empty; // new property

    }


}
