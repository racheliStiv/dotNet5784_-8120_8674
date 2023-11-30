namespace DO;
/// <summary>
/// Engineer entity - to describe the person responsible for carrying out the tasks
/// </summary>
/// <param name="Id"></param>
/// <param name="name"></param>
/// <param name="Email"></param>
/// <param name="level"></param>
/// <param name="Cost"></param>
public record Engineer
(
    int Id,
    string? Name = null,
    string? Email = null,
    DO.EngineerExperience? Level = null,
    double? Cost = null
)
{
    public Engineer() : this(0) { }     //empty ctr

}



