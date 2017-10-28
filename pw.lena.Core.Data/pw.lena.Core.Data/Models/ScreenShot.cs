using System;

namespace pw.lena.Core.Data.Models
{
    public class ScreenShot
    {
        public string GUID { get; set; }
        public DateTime dateCreate { get; set; }        
        public DateTime synchronizeTime { get; set; }
        public bool IsSynchronized { get; set; }
        public bool IsActive { get; set; }
        public long DeviceID { get; set; }
        public long QueueCommandID { get; set; }
        public byte[] ImageScreen { get; set; }

    }
}
