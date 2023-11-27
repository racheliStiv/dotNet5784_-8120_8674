namespace Dal;
using DalApi;
using DO;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task t = new(newId, item.Alias, item.Description, item.CreatedAtDate, item.RequiredEffortTime, item.IsMilestone, item.Complexity, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId);
        DataSource.Tasks.Add(t);
        return newId;
    }

    public void Delete(int id)
    {
        if (!DataSource.Tasks.Exists(x => x.Id == id) || DataSource.Dependencies.Exists(x => x.DependensOnTask == id))
            throw new Exception("This object cannot be deleted");
        DataSource.Tasks.Remove(DataSource.Tasks.Find(x => x.Id == id)!);  
    }

    public Task? Read(int id)
    {
        Task? t = DataSource.Tasks.Find(x => x.Id == id);
        return t;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) == null)
            throw new Exception($"Task with ID={item.Id} not exists");
        Delete(item.Id);
        Create(item);
    }
}
