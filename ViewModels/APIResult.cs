using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class APIResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
        public int StatusCode { get; set; }
    }
}
