using MRCD.Domain.Common;

namespace MRCD.Domain.Parent;

public static class ParentAssignmentRules
{
    public static Result ValidateCount(
        int count
    ) => count > 2
        ? Result.Failure("Ya se ha asignado el máximo de padre/padrinos al confirmando")
        : Result.Success();
}
