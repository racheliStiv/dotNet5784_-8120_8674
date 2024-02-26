using BlApi;
using BO;
using BL;

namespace BlImplementation;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;
    private IBl s_bl = BlApi.Factory.Get();


    //get BO task and create new DO task
    public int Create(Task boTask)
    {
        try
        {
            if (IBl.Status == ProjectStatus.AFTER)
                throw new BOCannotAddNewOne("impossible add task in this status");
            return _dal.Task.Create(BO_to_DO(boTask));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
        catch (BOCannotAddNewOne ex)
        {
            throw new BOCannotAddNewOne(ex.Message);
        }

        catch (BOInvalidDetailsException ex)
        {
            throw new BOInvalidDetailsException(ex.Message);
        }
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
        catch (DO.DalDeletionImpossibleException ex)
        {
            throw new BODeletionImpossibleException(ex.Message);
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
            return DO_to_BO(_dal.Task.Read(id)) ??
                throw new BODoesNotExistException($"Task with ID : {id} is not exsist");
        }
        catch (BODoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
    }
    //get BO task and update the DO task
    public void Update(Task boTask)
    {
        try
        {
            if (IBl.Status == ProjectStatus.AFTER)
            {
                DO.Task? origin_task = _dal.Task.Read(boTask.Id);
                if (origin_task == null) { throw new BODoesNotExistException("Task undefined"); }
                //initialize only one time!
                if (((origin_task.StartDate != null) && (origin_task.StartDate != boTask.StartDate))
                   || ((origin_task.CompleteDate != null) && (origin_task.CompleteDate != boTask.CompletedDate))
                   || (origin_task.StartDate == null && boTask.CompletedDate != null)
                   || (boTask.StartDate != null && boTask.CompletedDate != null && boTask.CompletedDate <= origin_task.StartDate)
                   || boTask.StartDate < IBl.StartDate
                   || boTask.Duration != origin_task.Duration
                   || boTask.AllDependencies != null && boTask.AllDependencies.Select(dep => dep.Id) != _dal.Dependency.ReadAll(dep => dep!.DependentTask == origin_task.Id).Select(dep => dep!.Id)
                   || boTask.Engineer != null && ((DO.EngineerExperience)boTask.ComplexityLevel! != origin_task.Complexity || _dal.Engineer.Read(boTask.Engineer.Id) == null)
                   || boTask.StartDate != null && boTask.Engineer != null && (boTask.Engineer.Id != origin_task.EngineerId) || (_dal.Engineer.Read(boTask.Engineer!.Id)!.Level) < (DO.EngineerExperience)boTask.ComplexityLevel!)
                    throw new BOInvalidUpdateException("can't update on AFTER create luz");
            }
            if (IBl.Status == ProjectStatus.BEFORE)
            {
                if (boTask.PlannedFinishDate != null
                    || boTask.PlannedStartDate != null
                    || boTask.StartDate != null
                    || boTask.CompletedDate != null
                    || boTask.Engineer != null)
                    throw new BOInvalidUpdateException("can't update on BEFORE create luz");
            }
            if (IBl.Status == ProjectStatus.IN)
            {
                if (boTask.PlannedStartDate < IBl.StartDate
                    || boTask.Engineer != null
                    || boTask.StartDate != null
                    || boTask.CompletedDate != null)
                    throw new BOInvalidUpdateException("can't update on IN create luz");
            }
            //update the task in dal 
            _dal.Task.Update(BO_to_DO(boTask));
            IsCreatedSchedule();
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new BOInvalidUpdateException(ex.Message);
        }
        //update the task status
        boTask.Status = CalcStatus(boTask.Id);
    }

    private void IsCreatedSchedule()
    {
        IEnumerable<DO.Task?> pllanedStartDateTasks = _dal.Task.ReadAll(t => t.ScheduledDate != null);
        if (pllanedStartDateTasks.Count() == _dal.Task.ReadAll().Count())
            s_bl.CreateSchedule();
    }
    //change from BO task to DO task
    private DO.Task BO_to_DO(Task boTask)
    {
        try
        {
            //valid check
            Valid(boTask);

            //calculate field
            if (boTask.PlannedStartDate != null && boTask.PlannedFinishDate == null)
            {
                DateTime planS = boTask?.PlannedStartDate ?? DateTime.MinValue;
                boTask!.PlannedFinishDate = planS.Add(boTask.Duration ?? TimeSpan.MinValue);
            }
            //create dal Task
            DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.StartDate, boTask.PlannedStartDate, boTask.Duration, boTask.PlannedFinishDate, boTask.CompletedDate, boTask.Product, boTask.Remarks, boTask.Engineer?.Id, (DO.EngineerExperience?)boTask.ComplexityLevel);
            IEnumerable<DO.Dependency?> origin_deps = _dal.Dependency.ReadAll(d => d.DependentTask == boTask.Id);
            foreach (var dependency in origin_deps)
            {
                if (dependency != null) _dal.Dependency.Delete(dependency.Id);
            }
            if (boTask.AllDependencies != null)
            {

                foreach (var dep in boTask.AllDependencies)
                {
                    DO.Dependency doDep = new DO.Dependency(0, boTask.Id, dep.Id);
                    _dal.Dependency.Create(doDep);
                }
            }
            return doTask;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //change from DO task to BO task
    private Task? DO_to_BO(DO.Task? doTask)
    {
        if (doTask == null) return null;
        IEnumerable<DO.Dependency> dependencies = _dal!.Dependency.ReadAll(t => t!.DependentTask == doTask.Id)!;
        List<TaskInList> allDependencies = dependencies
     .Where(t => t.DependensOnTask.HasValue)
     .Select(t => _dal!.Task.Read(t.DependensOnTask!.Value))
     .Where(t => t != null)
     .Select(task => new TaskInList()
     {
         Id = task!.Id,
         Description = task.Description,
         Alias = task.Alias,
         Status = CalcStatus(task.Id)
     })
     .ToList();
        EngineerInTask? eit = null;
        if (doTask.EngineerId != null)
        {
            eit = new EngineerInTask();
            eit.Id = doTask.Id;
            eit.Name = _dal.Engineer.Read(doTask.EngineerId.Value)!.Name;
        }
        Task boTask = new() { Id = doTask.Id, Description = doTask.Description, Alias = doTask.Alias, Status = CalcStatus(doTask.Id), CreatedAtDate = doTask.CreatedAtDate, AllDependencies = allDependencies, PlannedStartDate = doTask.ScheduledDate, StartDate = doTask.StartDate, PlannedFinishDate = doTask.DeadlineDate, CompletedDate = doTask.CompleteDate, Product = doTask.Product, Duration = doTask.Duration, Remarks = doTask.Remarks, Engineer = eit, ComplexityLevel = (EngineerExperience?)doTask.Complexity };

        return boTask;
    }

    //function to check engineer validation
    private void Valid(Task? t)
    {
        if (t == null)
            throw new BOCanNotBeNullException("missing task");
        if (t.Alias == null || t.Duration == null || t.ComplexityLevel == null)
            throw new BOCanNotBeNullException("missing details for task");
        if (t.Duration.Value <= TimeSpan.Zero || (int)t.ComplexityLevel > 4 || t.ComplexityLevel < 0)
            throw new BOInvalidDetailsException("invalid details for task");
        if (t.StartDate != null)
        {
            if (t.StartDate < IBl.StartDate)
                throw new BOInvalidUpdateException("cann't beggin task before update start date of project");
            if (t.AllDependencies != null && t.AllDependencies.Count > 0)
            {
                if (!t.AllDependencies.All(dep => _dal.Task.Read(dep.Id) != null))
                    throw new BOInvalidUpdateException("invalid dependencies");
                if (!IsValidDep(t.Id, t.AllDependencies.Select(t => t.Id)))
                    throw new BOInvalidUpdateException("circle dependency");
                if (!t.AllDependencies.All(t => _dal.Task.Read(t.Id)!.StartDate != null))
                    throw new BOInvalidUpdateException("your dependencies didn't done yet");
                if (t.AllDependencies.All(dep => _dal.Task.Read(dep.Id)!.StartDate!.Value.Add(_dal.Task.Read(dep.Id)!.Duration ?? TimeSpan.Zero) < t.StartDate))
                    throw new BOInvalidUpdateException("start datr is impossible");
            }
        }
    }
    //function to check if dependency is correct(no circle)
    private bool IsValidDep(int taskId, IEnumerable<int>? allDep)
    {
        if (allDep?.Count() == 0)
            return true;
        if (allDep!.Contains(taskId))
            return false;
        return allDep!.All(depId =>
             IsValidDep(taskId, _dal.Dependency
                 .ReadAll(d => d!.DependentTask == depId)
                 .Where(dep => dep!.DependensOnTask != null)
                 .Select(dep => (int)dep!.DependensOnTask!)
                 .ToList()));
    }
    //return the status of task
    private BO.TaskStatus CalcStatus(int id)
    {
        DO.Task myTask = _dal.Task.Read(id)!;
        if (myTask.ScheduledDate != null)
            if (myTask.StartDate != null)
                if (myTask.CompleteDate != null)
                    return BO.TaskStatus.DONE;
                else
                    return BO.TaskStatus.STARTED;
            else
                return BO.TaskStatus.SCHEDULED;
        return BO.TaskStatus.UNSCHEDULED;
    }
}