using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public class SailsResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseUri { get; set; }
        public string Content { get; set; }
        public byte[] ContentBytes { get; set; }
    }
}
