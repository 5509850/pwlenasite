using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Models
{
    public class CurrentStatus
    {
        public long ChatID { get; set; }

        public int StatusID { get; set; }

        public string Data { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}