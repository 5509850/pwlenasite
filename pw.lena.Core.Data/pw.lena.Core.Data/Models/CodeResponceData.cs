using System.Collections.Generic;

namespace pw.lena.Core.Data.Models
{
    public class CodeResponceData<T> : CodeResponce where T : class
    {
        public List<T> data { get; set; }
    }
}
