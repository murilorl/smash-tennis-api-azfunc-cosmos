using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Functions.Responses
{
    //
    // Summary:
    //     An App.Functions.Responses.CreatedObjectResult that when executed performs content
    //     negotiation, formats the entity body, and will produce a Microsoft.AspNetCore.Http.StatusCodes.Status201Created
    //     response if negotiation and formatting succeed.
    public class CreatedObjectResult : ObjectResult
    {
        //
        // Summary:
        //     Initializes a new instance of the App.Functions.Responses.CreatedObjectResult class.
        //
        // Parameters:
        //   value:
        //     The content to format into the entity body.
        public CreatedObjectResult(object value) : base(value)
        {
            this.StatusCode = StatusCodes.Status201Created;
        }
    }
}