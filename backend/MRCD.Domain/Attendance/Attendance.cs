namespace MRCD.Domain.Attendance;

public sealed class Attendance
{
    private Attendance() { }
    public Guid ID { get; private set; }
    public Guid UserId { get; private set; }
    public Guid PersonId { get; private set; }
    public bool IsAttendance { get; private set; }
    public DateOnly Date { get; private set; }

    public static Attendance Create(
        Guid user,
        Guid person,
        bool isAttendance,
        DateOnly date
    ) => new()
    {
        ID = Guid.NewGuid(),
        UserId = user,
        PersonId = person,
        IsAttendance = isAttendance,
        Date = date
    };
}