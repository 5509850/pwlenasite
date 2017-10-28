using SQLite.Net.Attributes;
using System;

namespace pw.lena.Core.Data.Models.SQLite
{
    [Table("CodeResponce")]
    public class CodeResponceSQL : CodeResponce
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Date { get; set; }        
    }
}
