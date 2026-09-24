using MRCD.Application.Person.Add;
using MRCD.Application.Services;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Services;

internal sealed class PersonFactory
{
    public Result<Domain.Person.Person> Create(
        AddPersonCommand command
    ) => Domain.Person.Person.Create(
        command.Name,
        StringNormalizer.NormalizeString(command.Name),
        command.IsMasculine,
        command.IsSunday,
        command.DOB,
        command.DegreeId,
        string.IsNullOrWhiteSpace(command.Phone) ? null : command.Phone.Trim(),
        command.Address
    );
}
