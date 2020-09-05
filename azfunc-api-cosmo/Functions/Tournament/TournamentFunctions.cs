using App.Functions.Responses;
using App.Model;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.Functions
{
    public class TournamentFunctions
    {
        private readonly ITournamentService _service;
        public TournamentFunctions(ITournamentService service)
        {
            _service = service;
        }

        [FunctionName("TournamentCreate")]
        public async Task<IActionResult> Post(
     [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tournaments")]
            HttpRequest req,
     CancellationToken cts,
     ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Tournament tournament = JsonSerializer.Deserialize<Tournament>(requestBody);

            IActionResult returnValue;
            try
            {
                returnValue = new CreatedObjectResult(await _service.AddAsync(tournament));
            }
            catch (ArgumentNullException ane)
            {
                log.LogWarning(ane.ToString());
                returnValue = new BadRequestObjectResult(ane.Message);
            }
            catch (ArgumentException ae)
            {
                log.LogWarning(ae.ToString());
                returnValue = new BadRequestObjectResult(ae.Message);
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when creating the tournament. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("Um erro inesperado ocorreu ao criar o torneio. Por favor, entre em contato com o administrador.", e));
            }

            return returnValue;
        }

    }
}
