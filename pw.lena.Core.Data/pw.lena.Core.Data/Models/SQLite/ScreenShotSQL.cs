using SQLite.Net.Attributes;

namespace pw.lena.Core.Data.Models.SQLite
{
    [Table("ScreenShot")]
    public class ScreenShotSQL
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public long Date { get; set; }
        public string GUID { get; set; }
        public string dateCreate { get; set; }        
        public bool IsSynchronized { get; set; }
        public bool IsActive { get; set; }
        public long QueueCommandID { get; set; }
        public byte[] ImageScreen { get; set; }
    }
}
