using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPImpacta.Models
{
    public class RegisterUser
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public bool isAdministrator { get; set; }
    }
}
