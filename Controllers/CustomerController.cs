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

    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        [Authorize]
        [HttpGet]
        [Route("GetAllCustomers")]

        public Response GetAllCustomers()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetAllCustomers(connection);
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("GetCustomerByID/{ID}")]

        public Response GetCustomerByID(long ID)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetCustomerByID(connection, ID);
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("AddCustomer")]
        public Response AddCustomer(Customer Customer)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.AddCustomer(connection, Customer);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateCustomer")]
        public Response UpdateCustomer(Customer Customer)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.UpdateCustomer(connection, Customer);
            return response;
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteCustomer/{ID}/{DATA_DATE}")]
        public Response DeleteCustomer(long ID, DateOnly DATA_DATE)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("CrudConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.DeleteCustomer(connection, ID, DATA_DATE);
            return response;
        }

    }
}