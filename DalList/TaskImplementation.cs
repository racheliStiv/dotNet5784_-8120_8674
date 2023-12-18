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

        //create a new task & add its to tasks list
        Task t = new(newId, item.Alias, item.Description, item.CreatedAtDate, item.RequiredEffortTime, item.IsMilestone, item.Complexity, item.StartDate, item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId);
        DataSource.Tasks.Add(t);
        return newId;
    }

    public void Delete(int id)
    {
        //check if the ID number received does exist & does not appear in another list, throw ex in case not
        if (!DataSource.Tasks.Exists(x => x.Id == id) || DataSource.Dependencies.Exists(x => x.DependensOnTask == id))
            throw new Exception("This object cannot be deleted");

        //remove the task from tasks list
        DataSource.Tasks.Remove(DataSource.Tasks.Find(x => x.Id == id)!);
    }

    public Task? Read(int id)
    {
        //return the task of received id, return null in case id doesnt exsist
        Task? t = DataSource.Tasks.Find(x => x.Id == id);
        return t;
    }

    public List<Task> ReadAll()
    {
        //return all tasks list
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {

        //check if item exsist
        if (Read(item.Id) == null)
            throw new Exception($"Task with ID={item.Id} not exists");

        //delete the original task
        DataSource.Tasks.Remove(DataSource.Tasks.Find(x => x.Id == item.Id)!);

        //create the updated task
        DataSource.Tasks.Add(item);
    }
}

