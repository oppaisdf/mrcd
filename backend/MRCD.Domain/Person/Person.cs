using MRCD.Domain.Common;

namespace MRCD.Domain.Person;

public sealed class Person
{
    private Person() { }
    public Guid ID { get; private set; }
    public string Name { get; private set; } = default!;
    public string NormalizedName { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public bool IsMasculine { get; private set; }
    public bool IsSunday { get; private set; }
    public DateOnly DOB { get; private set; }
    public DateOnly RegistrationDate { get; private set; }
    public string? Parish { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public Guid LastDegreeId { get; private set; }

    private static Result ValidDOB(
        DateOnly dob
    )
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6));
        if (now.Year - dob.Year > 25)
            return Result.Failure("El confirmando es mayor para este grupo");
        if (now.Year - dob.Year < 14)
            return Result.Failure("El confirmando es muy joven para este sacramento");
        return Result.Success();
    }

    public static Result<Person> Create(
        string name,
        string normalizedName,
        bool isMasculine,
        bool isSunday,
        DateOnly dob,
        Guid degree,
        string? phone,
        string address
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Person>.Failure("El nombre del confirmando es obligaatorio");
        if (string.IsNullOrWhiteSpace(normalizedName))
            return Result<Person>.Failure("El nombre normalizado del confirmando es requerido");
        if (name.Trim().Length > 65 || normalizedName.Trim().Length > 65)
            return Result<Person>.Failure("El nombre del confirmando no puede exceder los 65 caracteres");
        if (string.IsNullOrWhiteSpace(address))
            return Result<Person>.Failure("La dirección no puede ser nula");
        if (address.Trim().Length > 100)
            return Result<Person>.Failure("La dirección no puede exceder los 100 caracteres");
        if (!ContactRules.IsName(normalizedName))
            return Result<Person>.Failure("El nombre del confirmando solo debe contener letras");
        if (!ContactRules.IsPhone(phone))
            return Result<Person>.Failure("El número telefónico no es válido");
        if (phone?.Trim().Length > 10)
            return Result<Person>.Failure("El número telefónico no puede exceder los 10 dígitos");
        var validDOB = ValidDOB(dob);
        if (!validDOB.IsSuccess && validDOB.Error is not null)
            return Result<Person>.Failure(validDOB.Error);
        return Result<Person>.Success(new()
        {
            ID = Guid.NewGuid(),
            Name = name.Trim(),
            NormalizedName = normalizedName.Trim(),
            IsMasculine = isMasculine,
            IsSunday = isSunday,
            DOB = dob,
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            Address = address.Trim(),
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)),
            IsActive = true,
            LastDegreeId = degree
        });
    }

    // Validate a detached copy so a failed update cannot alter a tracked entity.
    public Result Update(
        string? name,
        string? normalizedName,
        DateOnly? dob,
        bool? isActive,
        bool? isSunday,
        string? parish,
        string? address,
        string? phone,
        Guid? degreeId
    )
    {
        if (!IsActive && isActive != true)
            return Result.Failure("Debe activar el confirmando antes de actualizar sus datos");
        var candidate = (Person)MemberwiseClone();
        var changes = new List<Func<Result>>();
        if (isActive.HasValue) changes.Add(() => candidate.SetActive(isActive.Value));
        if (isSunday.HasValue) changes.Add(() => candidate.SetDay(isSunday.Value));
        if (degreeId.HasValue) changes.Add(() => candidate.SetDegree(degreeId.Value));
        if (!string.IsNullOrWhiteSpace(name)) changes.Add(() => candidate.SetName(name, normalizedName!));
        if (dob.HasValue) changes.Add(() => candidate.SetDOB(dob.Value));
        if (!string.IsNullOrWhiteSpace(phone)) changes.Add(() => candidate.SetPhone(phone));
        if (!string.IsNullOrWhiteSpace(address)) changes.Add(() => candidate.SetAddress(address));
        if (!string.IsNullOrWhiteSpace(parish)) changes.Add(() => candidate.SetParish(parish));
        if (changes.Count == 0) return Result.Failure("No se han encontrado datos para actualizar");

        foreach (var change in changes)
        {
            var result = change();
            if (!result.IsSuccess) return result;
        }
        
        Name = candidate.Name;
        NormalizedName = candidate.NormalizedName;
        DOB = candidate.DOB;
        IsActive = candidate.IsActive;
        IsSunday = candidate.IsSunday;
        Parish = candidate.Parish;
        Address = candidate.Address;
        Phone = candidate.Phone;
        LastDegreeId = candidate.LastDegreeId;
        return Result.Success();
    }

    public Result SetName(
        string name,
        string normalized
    )
    {
        if (!ContactRules.IsName(normalized))
            return Result.Failure("El nombre del confirmando solo debe contener letras");
        if (name?.Trim().Equals(Name) == true)
            return Result.Failure("No se puede asignar el mismo nombre");
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("El nombre no puede estar vacío");
        if (string.IsNullOrWhiteSpace(normalized))
            return Result.Failure("El nombre normalizado no puede estar vacío");
        if (name.Trim().Length > 65 || normalized.Trim().Length > 65)
            return Result.Failure("El nombre del confirmando no puede exceder los 65 caracteres");
        if (IsActive)
        {
            Name = name.Trim();
            NormalizedName = normalized.Trim();
        }
        return Result.Success();
    }

    public Result SetDOB(
        DateOnly dob
    )
    {
        if (dob == DOB)
            return Result.Failure("No se puede asignar la misma fecha de nacimiento");
        var validDOB = ValidDOB(dob);
        if (validDOB.IsSuccess && IsActive) DOB = dob;
        return validDOB;
    }

    public Result SetPhone(
        string phone
    )
    {
        if (string.IsNullOrWhiteSpace(phone))
            return Result.Failure("El número telefónico no puede estar vacío");
        if (!ContactRules.IsPhone(phone))
            return Result.Failure("El número telefónico solo puede contener números");
        if (phone.Trim().Equals(Phone))
            return Result.Failure("No se puede asignar el mismo número telefónico");
        if (phone.Trim().Length > 10)
            return Result.Failure("El número telefónico no puede exceder los 10 dígitos");
        if (IsActive) Phone = phone.Trim();
        return Result.Success();
    }

    public Result SetAddress(
        string address
    )
    {
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure("La dirección no puede estar vacía");
        if (address.Trim().Equals(Address))
            return Result.Failure("No se puede asignar la misma dirección");
        if (address.Trim().Length > 100)
            return Result.Failure("La dirección no puede exceder los 100 caracteres");
        if (IsActive) Address = address.Trim();
        return Result.Success();
    }

    public Result SetParish(
        string parish
    )
    {
        if (string.IsNullOrWhiteSpace(parish))
            return Result.Failure("La parroquia de bautizo no puede estar vacía");
        if (parish.Trim().Equals(Parish))
            return Result.Failure("No se puede asignar la misma parroquia de bautizo");
        if (parish.Trim().Length > 30)
            return Result.Failure("La parroquia de bautizo no puede exceder los 30 caracteres");
        if (IsActive) Parish = parish.Trim();
        return Result.Success();
    }

    public Result SetDay(
        bool isSunday
    )
    {
        if (IsSunday == isSunday)
            return Result.Failure("El día es el mismo");
        if (IsActive) IsSunday = isSunday;
        return Result.Success();
    }

    public Result SetActive(
        bool isActive
    )
    {
        if (isActive == IsActive)
            return Result.Failure("El confirmando ya se encuentra en este estado");
        if (!isActive)
        {
            Address = null;
            Phone = null;
        }
        IsActive = isActive;
        return Result.Success();
    }

    public Result SetDegree(
        Guid degree
    )
    {
        if (degree == LastDegreeId)
            return Result.Failure("El grado académico ya ha sido asignado");
        if (IsActive) LastDegreeId = degree;
        return Result.Success();
    }
}