using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using API_WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace API_WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MerchantController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MerchantController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        [Authorize]
        [HttpGet]
        [Route("GetAllMerchants")]

        public Response GetAllMerchants()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetAllMerchants(connection);
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("GetMerchantByMerchantNo/{MerchantNo}")]

        public Response GetMerchantByMerchantNo(string MerchantNo)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetMerchantByMerchantNo(connection, MerchantNo);
            return response;
        }
        
        [Authorize]
        [HttpPost]
        [Route("AddMerchant")]
        public Response AddMerchant(Merchant Merchant)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.AddMerchant(connection, Merchant);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateMerchant")]
        public Response UpdateMerchant(Merchant Merchant)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.UpdateMerchant(connection, Merchant);
            return response;
        }

        
        /*[Authorize]
        [HttpDelete]
        [Route("DeleteMerchant/{MERCHANT_NO}/{DATA_DATE}")]
        public Response DeleteMerchant(string MERCHANT_NO, DateOnly REPORT_DATE, DateOnly LOAD_DATE, DateOnly DATA_DATE)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.DeleteMerchant(connection, MERCHANT_NO, REPORT_DATE, LOAD_DATE, DATA_DATE);
            return response;
        }
        */
    }
}