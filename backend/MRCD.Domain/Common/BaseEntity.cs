namespace MRCD.Domain.Common;

public partial class BaseEntity
{
    public Guid ID { get; protected init; }
    public string Name { get; protected set; } = default!;
}