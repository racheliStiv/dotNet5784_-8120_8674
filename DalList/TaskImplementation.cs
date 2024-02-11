namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{

    //CRUD of Task
    public int Create(Task item)
    {
        //get the next running key
        int newId = DataSource.Config.NextTaskId;

        //create a new task & add its to tasks collection
        Task t = new(newId, item.Alias, item.Description, item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.Duration, item.DeadlineDate, item.CompleteDate, item.Product, item.Remarks, item.EngineerId, item.Complexity);
        DataSource.Tasks.Add(t);
        return newId;
    }

    public void Delete(int id)
    {
        //check if the ID number received does exist & does not appear in another collection, throw ex in case not
        DO.Task? task_to_del = Read(id);
        if (task_to_del == null) throw new DalDoesNotExistException($"Task with ID={id} not exists");
        Dependency? dep = DataSource.Dependencies.FirstOrDefault(x => x.DependensOnTask == id);
        if (dep != null) throw new DalDeletionImpossibleException("This object cannot be deleted");

        //remove the task from tasks collection
        DataSource.Tasks.Remove(task_to_del);
    }

    public Task? Read(int id)
    {
        //return the task of received id, return null in case id doesnt exsist
        return DataSource.Tasks.FirstOrDefault(x => x.Id == id);
    }

    public Task? Read(Func<Task, bool> filter) //stage 2
    {
        //return the first task that meet the condition
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        //return all tasks in case there is no filter
        if (filter == null)
            return DataSource.Tasks.Select(x => x);

        //return the tasks that meet the condition 
        return DataSource.Tasks.Where(filter);
    }

    public void Update(Task item)
    {

        //check if item exsist
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} not exists");

        //delete the original task
        DataSource.Tasks.Remove(DataSource.Tasks.FirstOrDefault(x => x.Id == item.Id)!);

        //create the updated task
        DataSource.Tasks.Add(item);
    }

    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}

