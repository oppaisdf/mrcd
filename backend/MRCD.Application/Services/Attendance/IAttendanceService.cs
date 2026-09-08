using MRCD.Application.Attendance.DTOs;

namespace MRCD.Application.Services.Attendance;

public interface IAttendanceService
{
    IEnumerable<AttendanceDTO> GetConsumibleAttendances(
        IReadOnlyCollection<Domain.Person.Person> people,
        IReadOnlyCollection<Domain.Attendance.Attendance> attendances
    );
}