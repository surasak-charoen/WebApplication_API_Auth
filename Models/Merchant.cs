using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_WebApplication1.Models
{
    public class Merchant
    {

            public string MERCHANT_NO { get; set; } // Corresponds to [MERCHANT_NO] [varchar] (50) NOT NULL
            public decimal? YTD_FRAUD_VOLUME { get; set; } // Corresponds to [YTD_FRAUD_VOLUME] [decimal](20, 0) NULL
            public decimal? MTD_FRAUD_VOLUME { get; set; } // Corresponds to [MTD_FRAUD_VOLUME] [decimal](10, 0) NULL
            public decimal? YTD_FRAUD_VALUE { get; set; } // Corresponds to [YTD_FRAUD_VALUE] [numeric] (30, 2) NULL
            public decimal? MTD_FRAUD_VALUE { get; set; } // Corresponds to [MTD_FRAUD_VALUE] [numeric] (20, 2) NULL
            public DateOnly REPORT_DATE { get; set; } // Corresponds to [REPORT_DATE] [date] NOT NULL
            public DateTime LOAD_DATE { get; set; } // Corresponds to [LOAD_DATE] [datetime2] (7) NOT NULL
            public DateTime CREATE_DATETIME { get; set; } // Corresponds to [CREATE_DATETIME] [datetime2] (7) NULL
            public DateOnly DATA_DATE { get; set; } // Corresponds to [DATA_DATE] [date] NOT NULL

        }


    }
