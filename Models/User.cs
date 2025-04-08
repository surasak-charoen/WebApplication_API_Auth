using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace API_WebApplication1.Models
{
    public class User
    {

        /*public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Store the hashed password
        */
        public int Id { get; set; }
        public string EmployeeUsername { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public string LastUpdateUser { get; set; }
        public DateTime? LastUpdateDatetime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteDatetime { get; set; }
        public bool? EmpStatus { get; set; }
    }


}
