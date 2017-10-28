using SQLite.Net.Attributes;

namespace pw.lena.Core.Data.Models.SQLite
{
    [Table("Master")]
    public class MasterSQL : Master
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Date { get; set; }
    }
}
