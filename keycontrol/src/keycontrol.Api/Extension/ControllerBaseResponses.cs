using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Keys.Responses;
using keycontrol.Application.Reports.Responses;
using keycontrol.Application.Rooms.Responses;
using keycontrol.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneOf;
using OneOf.Types;

namespace keycontrol.Application.Extension;

public static class ControllerBaseResponses
{
    private static readonly Dictionary<string, int> errorTypeStatusCode = new(){
        {TypeError.Conflict.ToString(),StatusCodes.Status409Conflict},
        {TypeError.BadRequest.ToString(), StatusCodes.Status400BadRequest},
        {TypeError.NotFound.ToString(), StatusCodes.Status404NotFound}
    };
    public static IActionResult HandleResponseBase<T>(this ControllerBase controller, OneOf<T, AppError> response, Uri? createdUri = null)
    {
        return response.Match(
            result => createdUri != null ? controller.Created(createdUri, result) : controller.Ok(result),
            error =>
            {
                if(errorTypeStatusCode.TryGetValue(error.ErrorType, out var statusCode )){
                    return controller.Problem(statusCode: statusCode, title: error.Detail);
                }
                if (error.ErrorType == TypeError.ValidationError.ToString())
                {
                    return ValidationProblem(controller, error);
                }

                return controller.Problem();
            }
        );
    }
    private static ActionResult ValidationProblem(ControllerBase controller, AppError error)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var err in error.ValidationErrors)
        {
            modelStateDictionary.AddModelError(
                err.Description, err.Code
            );
        }

        return controller.ValidationProblem(modelStateDictionary);
    }
}