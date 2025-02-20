using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.EntityLayer.Response
{
    public class Response
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Result { get; set; }
        public ErrorsCode Error { get; set; }
        public int Count { get; set; }
    }
    public class ListResponse<T> : Response
    {
        public List<T> Data { get; set; }
    }
}
