using MRCD.Application.Abstracts.Handlers;

namespace MRCD.Application.Person.AddPerson;

public sealed record AddPersonCommand(
    Guid UserId,
    string Name,
    bool IsMasculine,
    bool IsSunday,
    DateOnly DOB,
    string Address,
    string? Phone,
    Guid DegreeId,
    IEnumerable<AddSimpleParentCommand> Parents
) : ICommand<Guid>;

public sealed record AddSimpleParentCommand(
    string Name,
    bool IsMasculine,
    string? Phone
);