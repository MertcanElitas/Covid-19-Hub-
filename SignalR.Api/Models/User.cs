using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }

        public virtual Team Team{ get; set; }
}
}
