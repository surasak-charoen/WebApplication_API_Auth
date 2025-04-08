using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using System.Data.SqlClient;

namespace API_WebApplication1.Models
{
    public class DAL
    {
        private string ConvertToIso8601UtcString(DateOnly date)
        {
            // Convert the DateOnly to a DateTime in UTC and then format it as ISO 8601
            return date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
        private string ConvertToIso8601UtcString(DateTime lOAD_DATE)
        {
            throw new NotImplementedException();
        }

        public Customer? customer { get; private set; }

        public Response GetAllCustomers(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT top 10 * FROM KLS_SML_CUSTOMER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Customer> customers = new List<Customer>();

            foreach (DataRow row in dt.Rows)
            {
                Customer customer = new Customer
                {
                    ID = row["ID"] != DBNull.Value ? Convert.ToInt64(row["ID"]) : 0,
                    CIF_NUMBER = row["CIF_NUMBER"] != DBNull.Value ? Convert.ToString(row["CIF_NUMBER"]) : string.Empty,
                    ID_TYPE = row["ID_TYPE"] != DBNull.Value ? Convert.ToInt64(row["ID_TYPE"]) : 0,
                    ID_NUMBER = row["ID_NUMBER"] != DBNull.Value ? Convert.ToString(row["ID_NUMBER"]) : string.Empty,
                    ID_COUNTRY = row["ID_COUNTRY"] != DBNull.Value ? Convert.ToString(row["ID_COUNTRY"]) : string.Empty,
                    DATA_DATE = row["DATA_DATE"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["DATA_DATE"])) : DateOnly.FromDateTime(DateTime.Now)
                };
                customers.Add(customer);
            }

            if (customers.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.Customers = customers;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.Customers = new List<Customer>(); // Initialize as an empty list instead of null
            }

            return response;
        }

        public Response GetCustomerByID(SqlConnection connection, long ID)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM KLS_SML_CUSTOMER WHERE ID = @ID", connection);
            da.SelectCommand.Parameters.AddWithValue("@ID", ID);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Customer Customer = new Customer
                {
                    ID = Convert.ToInt64(row["ID"]),
                    CIF_NUMBER = Convert.ToString(row["CIF_NUMBER"]),
                    ID_TYPE = Convert.ToInt64(row["ID_TYPE"]),
                    ID_NUMBER = Convert.ToString(row["ID_NUMBER"]),
                    ID_COUNTRY = Convert.ToString(row["ID_COUNTRY"]),
                    DATA_DATE = DateOnly.FromDateTime(Convert.ToDateTime(row["DATA_DATE"]))
                };

                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.Customers = new List<Customer> { Customer };
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.Customers = null;

            }

            return response;
        }

        public Response AddCustomer(SqlConnection connection, Customer Customer)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO KLS_SML_CUSTOMER(ID, CIF_NUMBER, ID_TYPE, ID_NUMBER, ID_COUNTRY, DATA_DATE) " +
                "VALUES( " +
                "@ID, @CIF_NUMBER, @ID_TYPE, @ID_NUMBER, @ID_COUNTRY, @DATA_DATE)", connection);

            cmd.Parameters.AddWithValue("@ID", Customer.ID);
            cmd.Parameters.AddWithValue("@CIF_NUMBER", Customer.CIF_NUMBER);
            cmd.Parameters.AddWithValue("@ID_TYPE", Customer.ID_TYPE);
            cmd.Parameters.AddWithValue("@ID_NUMBER", Customer.ID_NUMBER);
            cmd.Parameters.AddWithValue("@ID_COUNTRY", Customer.ID_COUNTRY);
            cmd.Parameters.AddWithValue("@DATA_DATE", Customer.DATA_DATE.ToDateTime(TimeOnly.MinValue));

            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 201;
                    response.StatusMessage = "Customer added.";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Data inserted.";
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                response.StatusCode = 500;
                response.StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return response;
        }


        public Response UpdateCustomer(SqlConnection connection, Customer Customer)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand(
                "UPDATE KLS_SML_CUSTOMER SET " +
                "CIF_NUMBER = @CIF_NUMBER, " +
                "ID_TYPE = @ID_TYPE, " +
                "ID_NUMBER = @ID_NUMBER, " +
                "ID_COUNTRY = @ID_COUNTRY " +
                "WHERE ID = @ID AND DATA_DATE = @DATA_DATE", connection);

            cmd.Parameters.AddWithValue("@ID", Customer.ID);
            cmd.Parameters.AddWithValue("@CIF_NUMBER", Customer.CIF_NUMBER);
            cmd.Parameters.AddWithValue("@ID_TYPE", Customer.ID_TYPE);
            cmd.Parameters.AddWithValue("@ID_NUMBER", Customer.ID_NUMBER);
            cmd.Parameters.AddWithValue("@ID_COUNTRY", Customer.ID_COUNTRY);
            cmd.Parameters.AddWithValue("@DATA_DATE", Customer.DATA_DATE.ToDateTime(TimeOnly.MinValue));

            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Customer updated.";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Data updated.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return response;
        }

        public Response DeleteCustomer(SqlConnection connection, long ID, DateOnly DATA_DATE)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("DELETE FROM KLS_SML_CUSTOMER WHERE ID = @ID AND DATA_DATE = @DATA_DATE", connection);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@DATA_DATE", DATA_DATE.ToDateTime(TimeOnly.MinValue));

            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Customer deleted";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Customer deleted";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return response;
        }

        public Merchant? merchant { get; private set; }
        public Response GetAllMerchants(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT top 10 * FROM MERCHANT_FRAUD_HISTORY", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Merchant> merchants = new List<Merchant>();

            foreach (DataRow row in dt.Rows)
            {
                Merchant merchant = new Merchant
                {
                    MERCHANT_NO = row["MERCHANT_NO"] != DBNull.Value ? Convert.ToString(row["MERCHANT_NO"]) : string.Empty,
                    YTD_FRAUD_VOLUME = row["YTD_FRAUD_VOLUME"] != DBNull.Value ? Convert.ToDecimal(row["YTD_FRAUD_VOLUME"]) : (decimal?)null,
                    MTD_FRAUD_VOLUME = row["MTD_FRAUD_VOLUME"] != DBNull.Value ? Convert.ToDecimal(row["MTD_FRAUD_VOLUME"]) : (decimal?)null,
                    YTD_FRAUD_VALUE = row["YTD_FRAUD_VALUE"] != DBNull.Value ? Convert.ToDecimal(row["YTD_FRAUD_VALUE"]) : (decimal?)null,
                    MTD_FRAUD_VALUE = row["MTD_FRAUD_VALUE"] != DBNull.Value ? Convert.ToDecimal(row["MTD_FRAUD_VALUE"]) : (decimal?)null,
                    REPORT_DATE = row["REPORT_DATE"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["REPORT_DATE"])) : DateOnly.FromDateTime(DateTime.Now),
                    LOAD_DATE = row["LOAD_DATE"] != DBNull.Value ? Convert.ToDateTime(row["LOAD_DATE"]) : DateTime.Now,
                    CREATE_DATETIME = row["CREATE_DATETIME"] != DBNull.Value ? Convert.ToDateTime(row["CREATE_DATETIME"]) : DateTime.Now,
                    DATA_DATE = row["DATA_DATE"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["DATA_DATE"])) : DateOnly.FromDateTime(DateTime.Now)
                };
                merchants.Add(merchant);
            }

            if (merchants.Count > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.Merchants = merchants;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.Merchants = new List<Merchant>(); // Initialize as an empty list instead of null
            }

            return response;
        }

        public Response GetMerchantByMerchantNo(SqlConnection connection, string MERCHANT_NO)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM MERCHANT_FRAUD_HISTORY WHERE MERCHANT_NO = @MERCHANT_NO", connection);
            da.SelectCommand.Parameters.AddWithValue("@MERCHANT_NO", MERCHANT_NO);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Merchant Merchant = new Merchant
                {
                    MERCHANT_NO = row["MERCHANT_NO"] != DBNull.Value ? Convert.ToString(row["MERCHANT_NO"]) : string.Empty,
                    YTD_FRAUD_VOLUME = row["YTD_FRAUD_VOLUME"] != DBNull.Value ? Convert.ToDecimal(row["YTD_FRAUD_VOLUME"]) : (decimal?)null,
                    MTD_FRAUD_VOLUME = row["MTD_FRAUD_VOLUME"] != DBNull.Value ? Convert.ToDecimal(row["MTD_FRAUD_VOLUME"]) : (decimal?)null,
                    YTD_FRAUD_VALUE = row["YTD_FRAUD_VALUE"] != DBNull.Value ? Convert.ToDecimal(row["YTD_FRAUD_VALUE"]) : (decimal?)null,
                    MTD_FRAUD_VALUE = row["MTD_FRAUD_VALUE"] != DBNull.Value ? Convert.ToDecimal(row["MTD_FRAUD_VALUE"]) : (decimal?)null,
                    REPORT_DATE = row["REPORT_DATE"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["REPORT_DATE"])) : DateOnly.FromDateTime(DateTime.Now),
                    LOAD_DATE = row["LOAD_DATE"] != DBNull.Value ? Convert.ToDateTime(row["LOAD_DATE"]) : DateTime.Now,
                    CREATE_DATETIME = row["CREATE_DATETIME"] != DBNull.Value ? Convert.ToDateTime(row["CREATE_DATETIME"]) : DateTime.Now,
                    DATA_DATE = row["DATA_DATE"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["DATA_DATE"])) : DateOnly.FromDateTime(DateTime.Now)
                };

                response.StatusCode = 200;
                response.StatusMessage = "Data Found";
                response.Merchants = new List<Merchant> { Merchant };
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No Data Found";
                response.Merchants = null;

            }

            return response;
        }

        public Response AddMerchant(SqlConnection connection, Merchant Merchant)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO MERCHANT_FRAUD_HISTORY(" +
                "MERCHANT_NO,YTD_FRAUD_VOLUME,MTD_FRAUD_VOLUME,YTD_FRAUD_VALUE,MTD_FRAUD_VALUE," + 
                "REPORT_DATE,LOAD_DATE,CREATE_DATETIME,DATA_DATE) " +
                "VALUES( " +
                "@MERCHANT_NO,@YTD_FRAUD_VOLUME,@MTD_FRAUD_VOLUME,@YTD_FRAUD_VALUE,@MTD_FRAUD_VALUE," +
                "@REPORT_DATE,@LOAD_DATE,@CREATE_DATETIME,@DATA_DATE)", connection);

            cmd.Parameters.AddWithValue("@MERCHANT_NO", Merchant.MERCHANT_NO);
            cmd.Parameters.AddWithValue("@YTD_FRAUD_VOLUME", Merchant.YTD_FRAUD_VOLUME);
            cmd.Parameters.AddWithValue("@MTD_FRAUD_VOLUME", Merchant.MTD_FRAUD_VOLUME);
            cmd.Parameters.AddWithValue("@YTD_FRAUD_VALUE", Merchant.YTD_FRAUD_VALUE);
            cmd.Parameters.AddWithValue("@MTD_FRAUD_VALUE", Merchant.MTD_FRAUD_VALUE);
            cmd.Parameters.AddWithValue("@REPORT_DATE", Merchant.REPORT_DATE.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@LOAD_DATE", DateTime.Now); // Use system datetime
            cmd.Parameters.AddWithValue("@CREATE_DATETIME", DateTime.Now); // Use system datetime
            cmd.Parameters.AddWithValue("@DATA_DATE", Merchant.DATA_DATE.ToDateTime(TimeOnly.MinValue));

            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 201;
                    response.StatusMessage = "Merchant added.";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Data inserted.";
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                response.StatusCode = 500;
                response.StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return response;
        }

        public Response UpdateMerchant(SqlConnection connection, Merchant Merchant)
        {
            Response response = new Response();

            // Convert LOAD_DATE to UTC and format it as required
            string formattedLoadDate = ConvertToIso8601UtcString(Merchant.LOAD_DATE);

            SqlCommand cmd = new SqlCommand(
                "UPDATE MERCHANT_FRAUD_HISTORY SET " +
                "YTD_FRAUD_VOLUME = @YTD_FRAUD_VOLUME, " +
                "MTD_FRAUD_VOLUME = @MTD_FRAUD_VOLUME, " +
                "YTD_FRAUD_VALUE = @YTD_FRAUD_VALUE, " +
                "MTD_FRAUD_VALUE = @MTD_FRAUD_VALUE, " +
                "CREATE_DATETIME = @CREATE_DATETIME, " +
                " WHERE MERCHANT_NO = @MERCHANT_NO AND REPORT_DATE = @REPORT_DATE AND " +
                "LOAD_DATE = @LOAD_DATE AND DATA_DATE = @DATA_DATE", connection);

            cmd.Parameters.AddWithValue("@MERCHANT_NO", Merchant.MERCHANT_NO);
            cmd.Parameters.AddWithValue("@YTD_FRAUD_VOLUME", Merchant.YTD_FRAUD_VOLUME);
            cmd.Parameters.AddWithValue("@MTD_FRAUD_VOLUME", Merchant.MTD_FRAUD_VOLUME);
            cmd.Parameters.AddWithValue("@YTD_FRAUD_VALUE", Merchant.YTD_FRAUD_VALUE);
            cmd.Parameters.AddWithValue("@MTD_FRAUD_VALUE", Merchant.MTD_FRAUD_VALUE);
            cmd.Parameters.AddWithValue("@REPORT_DATE", Merchant.REPORT_DATE.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@LOAD_DATE", formattedLoadDate); // Use the formatted date
            cmd.Parameters.AddWithValue("@CREATE_DATETIME", DateTime.Now);
            cmd.Parameters.AddWithValue("@DATA_DATE", Merchant.DATA_DATE.ToDateTime(TimeOnly.MinValue));

            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Customer updated.";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Data updated.";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return response;
        }

 
    }

}