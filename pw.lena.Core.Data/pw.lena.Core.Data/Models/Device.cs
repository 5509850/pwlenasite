using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pw.lena.Core.Data.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        public string Token { get; set; }

        public bool IsActive { get; set; }

        public int TypeDeviceID { get; set; }

        public int codeA { get; set; }

        public int codeB { get; set; }

        public string AndroidIDmacHash { get; set; }

        public DateTime dataCreate { get; set; }

        public string Name { get; set; }
    }
}
