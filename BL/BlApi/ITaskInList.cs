using BO;

namespace BlApi;

public interface ITaskInList
{
    public IEnumerable<TaskInList?> GetAllTasksInList(Func<BO.Task, bool>? filter = null);
}
