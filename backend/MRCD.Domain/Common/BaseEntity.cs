namespace MRCD.Domain.Common;

public abstract partial class BaseEntity
{
    public Guid ID { get; protected init; }
    public string Name { get; protected set; } = default!;
}