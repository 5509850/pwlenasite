using System;

namespace pw.lena.Core.Data.Models
{
    public class PowerPCreport
    {
        public DateTime dateTimeOnPC { get; set; }
        public DateTime dateTimeOffPC { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }
}
