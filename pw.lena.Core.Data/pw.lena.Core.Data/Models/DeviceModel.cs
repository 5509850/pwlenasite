namespace pw.lena.Core.Data.Models
{
    public class DeviceModel
    {
        public string Name { get; set; }

        public int TypeDeviceID { get; set; }

        public string Token { get; set; }

        public string AndroidIDmacHash { get; set; }
        
        public int codeA { get; set; }
        public int codeB { get; set; }
    }
}
