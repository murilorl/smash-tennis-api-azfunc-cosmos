using App.Functions.Responses;
using App.Models.Users;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
                if (guid != null)
                {
                    User user = await _userService.GetById(Guid.Parse(guid));
                    if (user != null)
                    {
                        actionResult = new OkObjectResult(user);
                    }
                    else
                    {
                        return new NotFoundResult();
                    }
                }
                else
                {
                    actionResult = new OkObjectResult(await _userService.GetAllAsync(req.QueryString.ToString()));
                }
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
                returnValue = new CreatedObjectResult(await _userService.AddAsync(user));
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

        [FunctionName("UsersUpdate")]
        public async Task<IActionResult> Put(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonSerializer.Deserialize<User>(requestBody);

            try
            {
                await _userService.UpdateAsync(guid, user);
                returnValue = new NoContentResult();
            }
            catch (ArgumentException ae)
            {
                log.LogWarning(ae.ToString());
                returnValue = new BadRequestObjectResult(ae.Message);
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when tried to update user. Message: {0}", e);
                returnValue = new BadRequestObjectResult("Um erro inesperado ocorreu ao criar o usuário. Por favor, entre em contato com o administrador.");
            }
            return returnValue;
        }

        [FunctionName("UsersPartialUpdate")]
        public async Task<IActionResult> PartialUpdate(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue = null;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            JsonPatchDocument<User> patchUser = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<User>>(requestBody);

            if (patchUser == null)
            {
                return new BadRequestObjectResult("The body of the request cannot be null");
            }

            try
            {
                await _userService.UpdatePartialAsync(guid, patchUser);
                returnValue = new NoContentResult();
            }
            catch (ApplicationException ae)
            {
                log.LogWarning(ae.ToString());
                returnValue = new NotFoundObjectResult(ae.Message);
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when partially updating user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when partially updating user. Message: {0}", e));
            }

            return returnValue;
        }

        [FunctionName("UsersDelete")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "users/{guid}")]
            HttpRequest req,
            CancellationToken cts,
            ILogger log,
            Guid guid)
        {
            IActionResult returnValue = null;

            try
            {
                await _userService.DeleteAsync(guid);
                returnValue = new OkResult();
            }
            catch (ApplicationException ae)
            {
                log.LogWarning(ae.ToString());
                returnValue = new NotFoundObjectResult(ae.Message);
            }
            catch (Exception e)
            {
                log.LogError("An exception occurred when deleting user. Message: {0}", e);
                returnValue = new BadRequestObjectResult(String.Format("An exception occurred when deleting user. Message: {0}", e));
            }
            return returnValue;
        }
    }
}
