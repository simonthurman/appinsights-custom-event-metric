using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace asda_func_app_complete
{
    public class datareqcomplete
    {

        private TelemetryClient telemetryClient;

        public datareqcomplete(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
           
        }
    

        [FunctionName("datareqcomplete")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string datareqnum = req.Query["datareqnum"];

            log.LogInformation($"Data request {datareqnum} is now complete.");

            var myevent = new EventTelemetry($"Data Request Complete {datareqnum}");

            telemetryClient.TrackEvent(myevent);
            
            var myMetric = new MetricTelemetry();
            myMetric.Name = "data requests";
            myMetric.Sum = 2;
            telemetryClient.TrackMetric(myMetric);

            telemetryClient.TrackTrace("tracking");

            string responseMessage = string.IsNullOrEmpty(datareqnum)
                ? "This HTTP triggered function executed successfully. Pass a data request job number in the query string or in the request body for a personalized response."
                : $"Data request:, {datareqnum} is now compete.";

            return Task.FromResult<IActionResult>(new OkResult());
        }
    }
}
