namespace DO;

public record Engineer
(
    int Id,
    string? name = null,
    string? Email = null,
    DO.EngineerExperience? level = null,
    double? Cost = null
)
{
    public Engineer() : this(0) { }
}



