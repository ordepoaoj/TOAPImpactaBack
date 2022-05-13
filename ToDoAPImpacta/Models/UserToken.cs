using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPImpacta.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string userName { get; set; }
    }
}
