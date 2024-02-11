using DO;

namespace BlImplementation;
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = Factory.Get;

    //get BO task and create new DO task
    public int Create(Task boTask)
    {
        DO.Task doTask = BO_to_DO(boTask);
        int idTask = _dal.Task.Create(doTask);
        return idTask;
    }

    //get task id & delete its from DO lay
    public void Delete(int id)
    {
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
    }

    //get all tasks from DO lay
    public IEnumerable<Task?> GetAllTasks(Func<Task, bool>? filter)
    {
        //get all Tasks from DO
        IEnumerable<DO.Task?> do_tasks = _dal.Task.ReadAll();

        //change each Task to BO
        IEnumerable<Task?> bo_tasks = do_tasks.Where(t => t != null).Select(t => DO_to_BO(t!));

        //checks which tasks meet the filter
        if (filter != null)
        {
            bo_tasks = bo_tasks.Where(filter!);
        }
        return bo_tasks;
    }

    //לא הבנתי מה הפו הזאת אמורה להחזיר
    public Task GetTaskDetails(Task boTask)
    {
        return boTask;
    }

    //get BO task and update the DO task
    public void Update(Task boTask)
    {
        try
        {
            _dal.Task.Update(BO_to_DO(boTask));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
    }

    //change from BO task to DO task
    private DO.Task BO_to_DO(Task boTask)
    {
        DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.StartDate, boTask.PlannedStartDate, boTask.Duration, boTask.PlannedFinishDate, boTask.ComletedDate, boTask.Product, boTask.Remarks, boTask.Engineer?.Id, (DO.EngineerExperience)boTask.ComplexityLevel);
        return doTask;
    }

    //change from DO task to BO task
    private Task DO_to_BO(DO.Task doTask)
    {

        IEnumerable<DO.Dependency> allDependencies = _dal!.Dependency.ReadAll();
        allDependencies = allDependencies.Where(t => t.DependensOnTask == doTask.Id);
        //BO.Status status= UNSCHEDULED;
        Task boTask = new(doTask.Id, doTask.Description, doTask.Alias, status, doTask.CreatedAtDate, allDependencies, doTask.ScheduledDate, doTask.StartDate, doTask.DeadlineDate, doTask.CompleteDate, doTask.Product, doTask.Duration, doTask.Remarks, doTask.EngineerId, doTask.Complexity);
        return boTask;
    }
}

