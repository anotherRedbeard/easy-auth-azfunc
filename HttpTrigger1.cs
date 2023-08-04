using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace easyauth.function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run( [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] 
            HttpRequest req, ILogger log)
        {
            try
            {

                log.LogInformation("C# HTTP trigger function processed a request.");

                // Retrieve user identity and claims from the request headers
                if (req.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var principalHeaderValue))
                {
                    //log the header value
                    log.LogInformation($"X-MS-CLIENT-PRINCIPAL: {principalHeaderValue[0]}");
                    // Decode the base64-encoded JSON string
                    string principalValue = Encoding.UTF8.GetString(Convert.FromBase64String(principalHeaderValue[0]));

                    //log principalValue
                    log.LogInformation($"principalValue: {principalValue}");

                    // Deserialize the JSON string using Newtonsoft.Json
                    var principal = JsonConvert.DeserializeObject<PrincipalValue>(principalValue);

                    // Access user details
                    string userId = principal?.Claims?.Find(c => c.Type == "preferred_username")?.Value;
                    string[] userRoles = principal?.Claims?.FindAll(c => c.Type == "roles")?.Select(c => c.Value)?.ToArray();

                    // Loop through all the claims and include them in the response
                    var claims = principal?.Claims;
                    StringBuilder sb = new StringBuilder();
                    if (claims != null)
                    {
                        foreach (var claim in claims)
                        {
                            sb.AppendLine($"{claim.Type}: {claim.Value}");
                        }
                    }

                    // Your business logic here...

                    string message = $"Hello, authenticated user! UserId: {userId}, UserRoles: {string.Join(",", userRoles)}";

                    // Test if the user is in a specific role (e.g., "Reader")
                    bool isReader = principal?.IsUserInRole("Reader") ?? false;
                    message += $", IsReader: {isReader}\n\nClaims:\n{sb.ToString()}";

                    return new OkObjectResult(message);
                }
                else
                {
                    // Print out all headers
                    foreach (var header in req.Headers)
                    {
                        log.LogInformation($"{header.Key}: {header.Value}");
                    }
                    
                    return new OkObjectResult("Header not found");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while processing the request: {Message}", ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}