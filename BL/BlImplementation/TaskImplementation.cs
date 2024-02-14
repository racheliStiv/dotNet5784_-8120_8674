using BlApi;
using BO;

namespace BlImplementation;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    //get BO task and create new DO task
    public int Create(Task boTask)
    {
        return _dal.Task.Create(BO_to_DO(boTask));
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

    //
    public Task GetTaskDetails(int id)
    {
        try
        {
            return DO_to_BO(_dal.Task.Read(id)) ?? throw new BO.BODoesNotExistException($"can't get task details of: ${id}");
        }
        catch (Exception ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
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
    private Task? DO_to_BO(DO.Task? doTask)
    {
        if (doTask == null) return null;

        IEnumerable<DO.Dependency> dependencies = _dal!.Dependency.ReadAll(t => t!.DependentTask == doTask.Id)!;

        List<TaskInList> allDependencies = dependencies
     .Where(t => t.DependensOnTask.HasValue)
     .Select(t => _dal!.Task.Read(t.DependensOnTask!.Value))
     .Select(task => new TaskInList()
     {
         Id = task!.Id,
         Description = task.Description,
         Alias = task.Alias,
         Status = CalcStatus(task.Id) // אם המשתנה קיים מחוץ לפונקציה ומופעל
                                      // Status = task.Status // אם הסטטוס קשור למשימה עצמה
     })
     .ToList();

        Task boTask = new() { Id = doTask.Id, Description = doTask.Description, Alias = doTask.Alias, Status = CalcStatus(doTask.Id), CreatedAtDate = doTask.CreatedAtDate, AllDependencies = allDependencies, PlannedStartDate = doTask.ScheduledDate, StartDate = doTask.StartDate, PlannedFinishDate = doTask.DeadlineDate, ComletedDate = doTask.CompleteDate, Product = doTask.Product, Duration = doTask.Duration, Remarks = doTask.Remarks, 
            Engineer = _dal.Engineer.Read(doTask.EngineerId), ComplexityLevel = (BO.EngineerExperience?)doTask.Complexity };
    
        return boTask;
    }

    //function to check engineer validation
    private static void Valid(Engineer? e)
    {
        if (e == null)
            throw new BO.BOCanNotBeNullException("missing engineer");
        if (e?.Id == null || e.Name == null || e.Email == null || e?.Level == null || e?.Cost == null)
            throw new BO.BOCanNotBeNullException("missing details for engineer");
        if (
        e.Name == "" ||
        e.Cost < 0)
            throw new BO.BOInvalidDetailsException("invalid details for engineer");
    }

    //return the status of task
    private Status CalcStatus(int id)
    {
        DO.Task myTask = _dal.Task.Read(id)!;
        if (myTask.ScheduledDate != null)
            if (myTask.StartDate != null)
                if (myTask.CompleteDate != null)
                    return Status.DONE;
                else
                    return Status.STARTED;
            else
                return Status.SCHEDULED;
        return Status.UNSCHEDULED;
    }
    public void updateStartDate(int id, DateTime? startDate)
    {
        Task t = GetTaskDetails(id);
        if (t.AllDependencies.Count == 0)
    }

    //private void regularUpdate(string? desc, string? alias, string? product, string? remarks, EngineerExperience? exp)
    //{
    //    try
    //    {
    //        if ((int?)exp > 4 || exp < 0)
    //            throw new BO.BOInvalidDetailsException("experience out of range");   
    //        Task newT = new(this.)
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
}

