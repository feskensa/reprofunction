using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repro.Functions.Loader;
using U4.SmartAutomation.NuGet.Models;

namespace Repro.Functions
{
    public class Function
    {
        private readonly ILoader _loader;

        public Function(ILoader loader)
        {
            _loader = loader;
        }

        [FunctionName("Function")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            _loader.BindLogger(log);

            try
            {
                var instance = await _loader.LoadAsync("https://functionreprostorage.blob.core.windows.net/blobs/Repro.DLL.dll");

                return new OkObjectResult("");
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.Message, StatusCode = 500 };
            }
        }
    }
}
