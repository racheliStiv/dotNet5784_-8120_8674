
namespace DO;
/// <summary>
/// Dependency entity - to describe a relationship between 2 tasks
/// </summary>
/// <param name="Id"></param>
/// <param name="DependentTask"></param>
/// <param name="DependensOnTask"></param>
public record Dependency
(
    int Id,
    int? DependentTask,
    int? DependensOnTask
)
{
    public Dependency() : this(0, 0, 0) { }    //empty ctr
}