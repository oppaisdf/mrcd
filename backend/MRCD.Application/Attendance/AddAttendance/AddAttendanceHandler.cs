using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Attendance.Contracts;
using MRCD.Application.Person.Contracts;
using MRCD.Domain.Common;

namespace MRCD.Application.Attendance.AddAttendance;

internal sealed class AddAttendanceHandler(
    IAttendanceRepository repo,
    IPersonRepository person,
    IPersistenceContext save
) : ICommandHandler<AddAttendanceCommand>
{
    private readonly IAttendanceRepository _repo = repo;
    private readonly IPersonRepository _person = person;
    private readonly IPersistenceContext _save = save;

    public async Task<Result> HandleAsync(
        AddAttendanceCommand command,
        CancellationToken cancellationToken
    )
    {
        var personIsActive = await _person.ExistsActiveAsync(command.PersonId, cancellationToken);
        if (!personIsActive)
            return Result.Failure("El confirmando no existe o se encuentra inactivo");
        if (command.Date.Year != DateTime.UtcNow.Year)
            return Result.Failure("Solo se admiten fechas para el año en curso");
        var attendance = Domain.Attendance.Attendance.Create(
            command.UserId,
            command.PersonId,
            command.IsAttendance,
            command.Date
        );
        var alreadyExists = await _repo.AlreadyExistsAsync(attendance, cancellationToken);
        if (alreadyExists)
            return Result.Failure("Ya se ha pasado asistencia a este confirmando");
        _repo.Add(attendance);
        await _save.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}