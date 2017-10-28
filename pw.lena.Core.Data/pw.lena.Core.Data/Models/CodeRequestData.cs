using System.Collections.Generic;

namespace pw.lena.Core.Data.Models
{
    public class CodeRequestData<T> : CodeRequest where T : class 
    {
        public List<T> data { get; set; }
    }
}
