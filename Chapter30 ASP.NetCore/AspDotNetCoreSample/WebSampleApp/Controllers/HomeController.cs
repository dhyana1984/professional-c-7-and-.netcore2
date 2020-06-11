using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSampleApp.Controllers
{
    public class HomeController
    {
        private readonly ISampleService _service;
        public HomeController(ISampleService service)
        {
            _service = service;
        }

        public async Task Index(HttpContext context)
        {
            var sb = new StringBuilder();
            sb.Append("<ul>");
            sb.Append(string.Join(string.Empty,
              _service.GetSampleStrings().Select(s => s.Li()).ToArray()));
            sb.Append("</ul>");
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(sb.ToString());
        }
    }
}
