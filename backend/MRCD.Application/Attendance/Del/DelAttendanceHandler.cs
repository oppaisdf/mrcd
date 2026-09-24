using MRCD.Application.Logs;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.Del;

internal sealed class DelAttendanceHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    AuditLog<DelAttendanceHandler> logs
) : ICommandHandler<DelAttendanceCommand>
{
    public async Task<Result> HandleAsync(
        DelAttendanceCommand command,
        CancellationToken cancellationToken
    )
    {
        var personIsActive = await person.ExistsActiveAsync(command.PersonId, cancellationToken);
        if (!personIsActive)
            return Result.Failure("El confirmando no existe o está inactivo");
        var exists = await repo.ExistsAsync(command.PersonId, command.Date, cancellationToken);
        if (!exists)
            return Result.Failure($"No se ha pasado asistencia a este confirmando en la fecha {command.Date:dd/MM/yyyy}");
        await repo.DeleteAsync(command.PersonId, command.Date, cancellationToken);
        logs.Write(command.UserId, "Attendance {date} has been deleted to person {person}.", command.Date, command.PersonId);
        return Result.Success();
    }
}