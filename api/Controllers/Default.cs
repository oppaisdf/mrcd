using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public static class DefaultApiResponses
{
    internal class Response<T>
    {
        public required bool Success { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }

    internal class CreatedResponse
    {
        public int Id { get; set; }
    }

    public static ActionResult DefaultServerError(
        this ControllerBase controller,
        string consoleMessage = "",
        string message = "Internal Server Error :c"
    )
    {
        if (consoleMessage != "")
            Console.WriteLine(consoleMessage);
        return controller.StatusCode(500, new Response<object> { Success = false, Message = message });
    }

    public static ActionResult DefaultBadRequest(
        this ControllerBase controller,
        string message
    ) => controller.BadRequest(new Response<object> { Success = false, Message = message });

    public static ActionResult DefaultUnauthorized(
        this ControllerBase controller,
        string message
    ) => controller.Unauthorized(new Response<object> { Success = false, Message = message });

    public static ActionResult DefaultNotFound(
        this ControllerBase controller,
        string message
    ) => controller.NotFound(new Response<object> { Success = false, Message = message });

    public static ActionResult DefaultOk<T>(
        this ControllerBase controller,
        T data,
        string message = ""
    ) => controller.Ok(new Response<T> { Success = true, Message = message, Data = data });

    public static ActionResult DefaultCreated(
        this ControllerBase controller,
        string route,
        int id
    ) => controller.CreatedAtAction(route, new { id }, new Response<CreatedResponse> { Success = true, Message = "", Data = new CreatedResponse { Id = id } });

    public static ActionResult DefaultConflict(
        this ControllerBase controller,
        string message
    ) => controller.Conflict(new Response<object> { Success = false, Message = message });
}