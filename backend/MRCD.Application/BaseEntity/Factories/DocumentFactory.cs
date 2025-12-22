using MRCD.Application.Abstracts.Factories;
using MRCD.Domain.Common;
using MRCD.Domain.Document;

namespace MRCD.Application.BaseEntity.Factories;

internal sealed class DocumentFactory : IBaseEntityFactory<Document>
{
    public Result<Document> Create(
        string name
    ) => Document.Create(name);
}