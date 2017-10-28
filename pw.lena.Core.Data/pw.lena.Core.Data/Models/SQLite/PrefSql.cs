using SQLite.Net.Attributes;

namespace pw.lena.Core.Data.Models.SQLite
{

    [Table("Pref")]
    public class PrefSql
    {
        [PrimaryKey]
        public int Id { get; set; }      
        public string Value { get; set; }
    }
}
