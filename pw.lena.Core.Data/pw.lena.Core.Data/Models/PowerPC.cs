using System;

namespace pw.lena.Core.Data.Models
{
    public  class PowerPC
    {      
        public string GUID { get; set; }
        public DateTime dateTimeOnPC { get; set; }        
        public DateTime dateTimeOffPC { get; set; }
        public DateTime synchronizeTime { get; set; }
        public bool IsSynchronized { get; set; }
        public bool IsActive { get; set; }
        public long DeviceID { get; set; }
    }
}
