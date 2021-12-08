using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Helpers
{
    public class GenericResponse
    {
        public GenericResponse(string message, bool success, object response=null)
        {
            this.message = message;
            this.success = success;
            this.response = response;
        }
        public string message { get; set; }
        public bool success { get; set; }
        public object response { get; set; }
    }
}
