using App.Functions.Responses;
using App.Models.Users;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.Functions.Users
{
    public class UserFunctions
    {
        private readonly IUserService _userService;
        public UserFunctions(IUserService userService)
        {
            _userService = userService;
        }

        [FunctionName("UsersRead")]
        public async Task<IActionResult> Get(
               [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{guid?}")]
                HttpRequest req,
               ILogger log,
               string guid)
        {
            IActionResult actionResult = null;
            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();

            try
            {
                actionResult = new OkObjectResult(await _userService.GetUsersAsync(req.QueryString.ToString()));
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when creating a user. Message: {0}", e);
                actionResult = new BadRequestObjectResult(String.Format("Um erro inesperado ocorreu ao criar o usuário. Por favor, entre em contato com o administrador.", e));
            }

            return actionResult;
        }

        [FunctionName("UsersCreate")]
        public async Task<IActionResult> Post(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users")]
            HttpRequest req,
           CancellationToken cts,
           ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonSerializer.Deserialize<User>(requestBody);

            IActionResult returnValue;
            try
            {
                returnValue = new CreatedObjectResult(await _userService.AddItemAsync(user));
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
                log.LogError("An exception occurred when creating a user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("Um erro inesperado ocorreu ao criar o usuário. Por favor, entre em contato com o administrador.", e));
            }

            return returnValue;
        }
    }
}
