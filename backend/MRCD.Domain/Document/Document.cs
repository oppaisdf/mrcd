using MRCD.Domain.Common;

namespace MRCD.Domain.Document;

public sealed class Document : BaseEntity
{
    private Document() { }
    public static Result<Document> Create(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Document>.Failure("El nombre del documento no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result<Document>.Failure("El nombre del documento no puede exceder los 30 caracteres");
        return Result<Document>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim()
        });
    }

    public Result SetName(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("El nombre del documento no puede estar vacío");
        if (name.Trim().Length > 30)
            return Result.Failure("El nombre del documento no puede exceder los 30 caracteres");
        if (name.Trim().Equals(Name))
            return Result.Failure("El nombre ya está en uso");
        Name = name.Trim();
        return Result.Success();
    }
}