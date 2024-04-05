using BO;

namespace BlApi;

public interface ITaskInList
{
    public IEnumerable<TaskInList> GetAllTasksInList(IEnumerable<BO.Task>? tasks);
}
