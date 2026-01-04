using Microsoft.Extensions.Logging;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.DelAttendance;

internal sealed class DelAttendanceHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    ILogger<DelAttendanceHandler> logs
) : ICommandHandler<DelAttendanceCommand>
{
    private readonly IAttendanceRepository _repo = repo;
    private readonly IPersonRepository _person = person;
    private readonly ILogger<DelAttendanceHandler> _logs = logs;

    public async Task<Result> HandleAsync(
        DelAttendanceCommand command,
        CancellationToken cancellationToken
    )
    {
        var personIsActive = await _person.ExistsActiveAsync(command.PersonId, cancellationToken);
        if (!personIsActive)
            return Result.Failure("El confirmando no existe o está inactivo");
        var exists = await _repo.ExistsAsync(command.PersonId, command.Date, cancellationToken);
        if (!exists)
            return Result.Failure($"No se ha pasado asistencia a este confirmando en la fecha {command.Date:dd/MM/yyyy}");
        await _repo.DeleteAsync(command.PersonId, command.Date, cancellationToken);
        _logs.LogInformation("Attendance {date} has been deleted to person {person} by user {user}", command.Date, command.PersonId, command.UserId);
        return Result.Success();
    }
}