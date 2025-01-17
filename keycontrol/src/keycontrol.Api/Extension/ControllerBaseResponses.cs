using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Key.Responses;
using keycontrol.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneOf;
using OneOf.Types;

namespace keycontrol.Application.Extension;

public static class ControllerBaseResponses
{
    public static IActionResult RegisterResponseBase(this ControllerBase controller,
        OneOf<RegisterResponse, AppError> response)
    {
        return response.Match(
            result => controller.Created("Users/Guid", result),
            error =>
            {
                if (error.ErrorType == TypeError.Conflict.ToString())
                {
                    return controller.Problem(statusCode: StatusCodes.Status409Conflict, title: error.Detail);
                }
                if (error.ErrorType == TypeError.BadRequest.ToString())
                {
                    return controller.Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Detail);
                }
                if (error.ErrorType == TypeError.ValidationError.ToString())
                {
                    return ValidationProblem(controller, error);
                }

                return controller.Problem();
            }
        );
    }

    public static IActionResult LoginResponseBase(this ControllerBase controller,
        OneOf<LoginResponse, AppError> response)
    {
        return response.Match(
            result => controller.Ok(result),
            error =>
            {
                if (error.ErrorType == TypeError.Conflict.ToString())
                {
                    return controller.Problem(statusCode: StatusCodes.Status409Conflict, title: error.Detail);
                }
                if (error.ErrorType == TypeError.BadRequest.ToString())
                {
                    return controller.Problem(statusCode: StatusCodes.Status400BadRequest, title: error.Detail);
                }
                if (error.ErrorType == TypeError.ValidationError.ToString())
                {
                    return ValidationProblem(controller, error);
                }

                return controller.Problem();
            }
        );
    }

    public static IActionResult RegisterKeyResponseBase(this ControllerBase controller,
        OneOf<RegisterKeyResponse, AppError> response)
    {
        return response.Match(
            result => controller.Created("Keys/Guid", result),
            error =>
            {
                if (error.ErrorType == TypeError.Conflict.ToString())
                {
                    return controller.Problem(statusCode: StatusCodes.Status409Conflict, title: error.Detail);
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