using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPImpacta.Areas.Identity.Data
{
    public class ToDoAPImpactaUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
    }
}
