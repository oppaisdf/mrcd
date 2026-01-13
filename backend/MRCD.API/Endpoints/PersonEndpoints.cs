using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MRCD.API.Common;
using MRCD.API.DTOs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Person.AddPerson;
using MRCD.Application.Person.DTOs;
using MRCD.Application.Person.GetGeneralList;
using MRCD.Application.Person.GetPersonById;
using MRCD.Application.Person.UpdatePerson;

namespace MRCD.API.Endpoints;

internal static class PersonEndpoints
{
    public static void MapPersonEndpoints(
        this IEndpointRouteBuilder builder
    )
    {
        var app = builder
            .MapGroup("/api/v2/person")
            .WithTags("Person");

        app.MapPost("", async (
            [FromBody] AddPersonRequest request,
            ClaimsPrincipal user,
            ICommandHandler<AddPersonCommand, Guid> handler,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();

            var command = new AddPersonCommand(
                userId,
                request.Name,
                request.IsMasculine,
                request.IsSunday,
                request.DOB,
                request.Address,
                request.Phone,
                request.DegreeId,
                request.Parents
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                id => Results.Created($"/api/v2/person/{id}", id),
                e => e.Contains("no existe"),
                e => e.Contains("ya se ha")
            );
        })
        .WithName("AddPerson")
        .WithDisplayName("POST /AddPerson")
        .WithSummary("Agregar confirmando")
        .WithDescription("Crea un confirmando")
        .WithOpenApi()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapGet("/generallist", async (
            ClaimsPrincipal user,
            IQueryHandler<IEnumerable<GeneralListDTO>, GetGeneralListQuery> handler,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var query = new GetGeneralListQuery(userId);
            var result = await handler.HandleAsync(query, ct);
            ResultsMapper.ToHttp(
                result,
                l => Results.Ok(l)
            );
        })
        .WithName("GetGeneralList")
        .WithDisplayName("GET /GeneralList")
        .WithSummary("Obtener listado general")
        .WithDescription("Obtiene listado general de personas activas")
        .WithOpenApi()
        .Produces<IEnumerable<GeneralListDTO>>(StatusCodes.Status200OK);

        app.MapGet("{id}", async (
            Guid id,
            ClaimsPrincipal user,
            IQueryHandler<PersonDTO, GetPersonByIdQuery> handler,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var query = new GetPersonByIdQuery(
                userId,
                id
            );
            var result = await handler.HandleAsync(query, ct);
            ResultsMapper.ToHttp(
                result,
                p => Results.Ok(p),
                e => e.Contains("no existe")
            );
        })
        .WithName("GetPersonById")
        .WithDisplayName("GET /PersonById")
        .WithSummary("Obtener confirmando")
        .WithDescription("Obtiene detalles de confirmando por Id")
        .WithOpenApi()
        .Produces<PersonDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("{id}", async (
            Guid id,
            [FromBody] UpdatePersonRequest request,
            ClaimsPrincipal user,
            ICommandHandler<UpdatePersonCommand> handler,
            CancellationToken ct
        ) =>
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
                ResultsMapper.Unauthorized();
            var command = new UpdatePersonCommand(
                userId,
                id,
                request.Name,
                request.DOB,
                request.IsActive,
                request.IsSunday,
                request.Parish,
                request.Address,
                request.Phone,
                request.DegreeId
            );
            var result = await handler.HandleAsync(command, ct);
            ResultsMapper.ToHttp(
                result,
                () => Results.Ok(),
                e => e.Contains("no existe"),
                e => e.Contains("ya existe")
            );
        })
        .WithName("PatchPerson")
        .WithDisplayName("PATCH /PatchPerson")
        .WithSummary("Actualizar confirmando")
        .WithDescription("Actualiza al confirmando por Id")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}