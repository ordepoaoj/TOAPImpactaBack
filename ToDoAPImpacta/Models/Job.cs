using System;
using System.Collections.Generic;

#nullable disable

namespace ToDoAPImpacta.Models
{
    public partial class Job
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? InOperation { get; set; }
        public DateTime? FinallyDate { get; set; }
        public string User { get; set; }

        public virtual AspNetUser UserNavigation { get; set; }
    }
}
