
namespace DO;

public record Dependency
(
    int Id,
    int DependentDateTask,
    int DependensOnTask
)
{
    public Dependency() : this(0, 0, 0) { }
}