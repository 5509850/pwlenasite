using SQLite.Net.Attributes;

namespace pw.lena.Core.Data.Models.SQLite
{

    [Table("PowerPC")]
    public class PowerPCSQL
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public long Date { get; set; }
        public string GUID { get; set; }
        public string dateTimeOnPC { get; set; }
        public string dateTimeOffPC { get; set; }
        public bool IsSynchronized { get; set; }
        public bool IsActive { get; set; }
    }
}
