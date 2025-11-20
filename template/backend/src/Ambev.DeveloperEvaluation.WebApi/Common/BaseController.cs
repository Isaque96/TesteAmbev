using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public abstract class BaseController : ControllerBase
{
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)!.Value;

    protected IActionResult Ok<T>(T? data = default, string message = "Sucesso", bool success = true) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = success, Message = message });

    protected IActionResult Created<T>(string routeName, object routeValues, T? data = default, string message = "Sucesso", bool success = true) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Message = message, Success = success });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });
}
