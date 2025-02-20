using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.EntityLayer.Response
{
    public class ErrorResponse
    {
        public string Result { get; set; }
        public ErrorsCode Error { get; set; }
    }
    public class ErrorsCode
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
