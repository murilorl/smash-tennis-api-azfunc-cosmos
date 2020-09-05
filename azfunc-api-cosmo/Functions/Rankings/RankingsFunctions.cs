using App.Model;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Functions.Rankings
{
    public class RankingsFunctions
    {
        private readonly IRankingsService _service;
        public RankingsFunctions(IRankingsService service)
        {
            _service = service;
        }

        [FunctionName("RankingsRead")]
        public async Task<IActionResult> Get(
               [HttpTrigger(AuthorizationLevel.Function, "get", Route = "rankings/{guid?}")]
                HttpRequest req,
               ILogger log,
               string guid)
        {
            IActionResult actionResult = null;
            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();

            try
            {
                if (guid != null)
                {
                    Ranking ranking = await _service.GetById(Guid.Parse(guid));
                    if (ranking != null)
                    {
                        actionResult = new OkObjectResult(ranking);
                    }
                    else
                    {
                        return new NotFoundResult();
                    }
                }
                else
                {
                    actionResult = new OkObjectResult(await _service.GetAllAsync(req.QueryString.ToString()));
                }
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred. Message: {0}", e);
                actionResult = new BadRequestObjectResult(String.Format("Um erro inesperado ocorreu ao recuperar os rankings. Por favor, entre em contato com o administrador.", e));
            }

            return actionResult;
        }
    }
}
