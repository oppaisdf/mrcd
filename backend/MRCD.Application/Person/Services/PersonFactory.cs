using MRCD.Application.Person.Add;
using MRCD.Application.Services.Common;
using MRCD.Domain.Common;

namespace MRCD.Application.Person.Services;

internal sealed class PersonFactory(
    ICommonService normalizer
)
{
    public Result<Domain.Person.Person> Create(
        AddPersonCommand command
    ) => Domain.Person.Person.Create(
        command.Name,
        normalizer.NormalizeString(command.Name),
        command.IsMasculine,
        command.IsSunday,
        command.DOB,
        command.DegreeId,
        string.IsNullOrWhiteSpace(command.Phone) ? null : command.Phone.Trim(),
        command.Address
    );
}
