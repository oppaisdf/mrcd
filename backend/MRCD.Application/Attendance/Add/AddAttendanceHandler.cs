using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.Add;

internal sealed class AddAttendanceHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    IPersistenceContext save
) : ICommandHandler<AddAttendanceCommand>
{
    public async Task<Result> HandleAsync(
        AddAttendanceCommand command,
        CancellationToken cancellationToken
    )
    {
        var personIsActive = await person.ExistsActiveAsync(command.PersonId, cancellationToken);
        if (!personIsActive)
            return Result.Failure("El confirmando no existe o se encuentra inactivo");
        var attendance = Domain.Attendance.Attendance.Create(
            command.UserId,
            command.PersonId,
            command.IsAttendance,
            command.Date
        );
        if (!attendance.IsSuccess) return Result.Failure(attendance.Error!);
        var alreadyExists = await repo.AlreadyExistsAsync(attendance.Value!, cancellationToken);
        if (alreadyExists)
            return Result.Failure("Ya se ha pasado asistencia a este confirmando");
        repo.Add(attendance.Value!);
        await save.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}