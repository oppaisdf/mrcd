namespace api.Models.Filters;

public class PeopleFilter
{
    public required short Page { get; set; }
    public string? Name { get; set; }
    public bool? Gender { get; set; }
    public bool? Day { get; set; }
    public short? DegreeId { get; set; }
    public bool? IsActive { get; set; }
}