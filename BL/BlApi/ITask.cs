namespace BlApi;

public interface ITask
{
    public IEnumerable<Task?> GetAllTasks(Func<Task, bool>? filter);
    public Task GetTaskDetails(int id);
    public int Create(Task boTask);
    public void Delete(int id);
    public void Update(Task boTask);
}
