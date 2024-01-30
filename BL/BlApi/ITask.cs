namespace BlApi;

public interface ITask
{
    public IEnumerable<Task?> GetAllTasks(Func<Task, bool>? filter);
    public Task GetTaskDetails(Task Task);
    public void Create(Task Task);
    public void Delete(int id);
    public void Update(Task Task);


}
