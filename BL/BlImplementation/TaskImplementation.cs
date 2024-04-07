using BlApi;
using BO;

namespace BlImplementation;
internal class TaskImplementation : ITask
{

    //get BO task and create new DO task
    public int Create(BO.Task boTask)
    {
        try
        {
            if (Bl.Status == ProjectStatus.AFTER)
                throw new BOCannotAddNewOne("impossible to add task on this AFTER status");
            return Bl._dal.Task.Create(BO_to_DO(boTask));
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

            Bl._dal.Task.Delete(id);
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
    public IEnumerable<BO.Task?> GetAllTasks(Func<BO.Task, bool>? filter)
    {
        //get all Tasks from DO
        IEnumerable<DO.Task?> do_tasks = Bl._dal.Task.ReadAll();
        //change each Task to BO
        IEnumerable<BO.Task?> bo_tasks = do_tasks.Where(t => t != null).Select(t => DO_to_BO(t!));
        //checks which tasks meet the filter
        if (filter != null)
        {
            bo_tasks = bo_tasks.Where(filter!);
        }
        return bo_tasks;
    }
    public BO.Task GetTaskDetails(int id)
    {
        try
        {
            return DO_to_BO(Bl._dal.Task.Read(id)) ?? throw new BODoesNotExistException($"Task with ID = {id} is not exsist");
        }
        catch (BODoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
    }
    //get BO task and update the DO task
    public void Update(BO.Task boTask)
    {
        try
        {
            if (Bl.Status == ProjectStatus.AFTER)
            {
                DO.Task? origin_task = Bl._dal.Task.Read(boTask.Id);
                if (origin_task == null) { throw new BODoesNotExistException("Task undefined"); }

                //initialize only one time!
                if (
                  ((origin_task.StartDate != null) && (origin_task.StartDate != boTask.StartDate))
                 || ((origin_task.CompleteDate != null) && (origin_task.CompleteDate != boTask.CompletedDate))
                 || (origin_task.StartDate == null && boTask.CompletedDate != null)
                 || (boTask.StartDate != null && boTask.CompletedDate != null && boTask.CompletedDate < origin_task.StartDate)
                 || boTask.Duration != origin_task.Duration)
                    throw new BOInvalidUpdateException("can't update. on AFTER create luz");               
                if (boTask.Engineer != null && (Bl._dal.Engineer.Read(boTask.Engineer!.Id)!.Level) < (DO.EngineerExperience)boTask.ComplexityLevel!) throw new BOInvalidUpdateException("complexity in not enough");
                if (boTask.Engineer != null && ((DO.EngineerExperience)boTask.ComplexityLevel! != origin_task.Complexity || Bl._dal.Engineer.Read(boTask.Engineer.Id) == null)) throw new BOInvalidUpdateException("can't update comlexity after engineer init");
                if (boTask.Engineer == null && boTask.StartDate != null) throw new BOInvalidUpdateException($"can't start task before init engineer");
                if (boTask.StartDate < Factory.Get.BeginDate) throw new BOInvalidUpdateException("start date of task can't be before project begining");
                if (origin_task.StartDate != null && boTask.Engineer != null && (boTask.Engineer.Id != origin_task.EngineerId)) throw new BOInvalidUpdateException("can't change engineer after beginning");
            }
            if (Bl.Status == ProjectStatus.BEFORE)
            {
                if (boTask.PlannedFinishDate != null
                    || boTask.PlannedStartDate != null
                    || boTask.StartDate != null
                    || boTask.CompletedDate != null
                    || boTask.Engineer != null)
                    throw new BOInvalidUpdateException("can't update. on BEFORE create luz");
            }
            if (Bl.Status == ProjectStatus.IN)
            {
                if (boTask.Engineer != null
                    || boTask.StartDate != null
                    || boTask.CompletedDate != null)
                    throw new BOInvalidUpdateException("can't update. on IN create luz");
                if (boTask.PlannedStartDate < Factory.Get.BeginDate) throw new BOInvalidUpdateException("planning start date of task can't be before project begining");
            }

            //update the task in dal 
            Bl._dal.Task.Update(BO_to_DO(boTask));
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

    //check if it's last update before AFTER status of the projecr
    internal void IsCreatedSchedule()
    {
        if (Bl.Status == ProjectStatus.IN)
        {
            IEnumerable<DO.Task?> pllanedStartDateTasks = Bl._dal.Task.ReadAll(t => t.ScheduledDate != null);
            if (pllanedStartDateTasks.Count() == Bl._dal.Task.ReadAll().Count())
                Bl.Status = ProjectStatus.AFTER;
        }        
    }

    //change from BO task to DO task
    private DO.Task BO_to_DO(BO.Task boTask)
    {
        try
        {
            //valid check
            try { Valid(boTask); }
            catch (Exception) { throw; }

            //calculate field
            if (boTask.PlannedStartDate != null && boTask.PlannedFinishDate == null)
            {
                DateTime planS = boTask?.PlannedStartDate ?? DateTime.MinValue;
                boTask!.PlannedFinishDate = planS.Add(boTask.Duration ?? TimeSpan.MinValue);
            }
            //create dal Task
            DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, boTask.StartDate, boTask.PlannedStartDate, boTask.Duration, boTask.PlannedFinishDate, boTask.CompletedDate, boTask.Product, boTask.Remarks, boTask.Engineer?.Id, (DO.EngineerExperience?)boTask.ComplexityLevel);
            IEnumerable<DO.Dependency?> origin_deps = Bl._dal.Dependency.ReadAll(d => d.DependentTask == boTask.Id);
            foreach (var dependency in origin_deps)
            {
                if (dependency != null) Bl._dal.Dependency.Delete(dependency.Id);
            }
            if (boTask.AllDependencies != null && boTask.AllDependencies.Count > 0)
            {

                foreach (var dep in boTask.AllDependencies)
                {
                    DO.Dependency doDep = new DO.Dependency(0, boTask.Id, dep.Id);
                    Bl._dal.Dependency.Create(doDep);
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
    private BO.Task? DO_to_BO(DO.Task? doTask)
    {
        if (doTask == null) return null;
        IEnumerable<DO.Dependency> dependencies = Bl._dal!.Dependency.ReadAll(t => t!.DependentTask == doTask.Id)!;
        List<TaskInList> allDependencies = dependencies
     .Where(t => t.DependensOnTask.HasValue)
     .Select(t => Bl._dal!.Task.Read(t.DependensOnTask!.Value))
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
            eit.Id = doTask.EngineerId.Value;
            eit.Name = Bl._dal.Engineer.Read(doTask.EngineerId.Value)!.Name;
        }
        BO.Task boTask = new() { Id = doTask.Id, Description = doTask.Description, Alias = doTask.Alias, Status = CalcStatus(doTask.Id), CreatedAtDate = doTask.CreatedAtDate, AllDependencies = allDependencies, PlannedStartDate = doTask.ScheduledDate, StartDate = doTask.StartDate, PlannedFinishDate = doTask.DeadlineDate, CompletedDate = doTask.CompleteDate, Product = doTask.Product, Duration = doTask.Duration, Remarks = doTask.Remarks, Engineer = eit, ComplexityLevel = (EngineerExperience?)doTask.Complexity };

        return boTask;
    }

    //function to check engineer validation
    private void Valid(BO.Task? t)
    {
        if (t == null)
            throw new BOCanNotBeNullException("missing task");
        if (t.Alias == null || t.Duration == null || t.ComplexityLevel == null)
            throw new BOCanNotBeNullException("missing details for task");
        if (t.Duration.Value <= TimeSpan.Zero || (int)t.ComplexityLevel > 4 || t.ComplexityLevel < 0)
            throw new BOInvalidDetailsException("invalid details for task");
        if (t.StartDate != null)
        {
            if (t.AllDependencies != null && t.AllDependencies.Count > 0)
            {
                if (!t.AllDependencies.All(dep => Bl._dal.Task.Read(dep.Id)!.StartDate != null))
                    throw new BOInvalidUpdateException("your dependencies didn't done yet");
                if (t.AllDependencies.All(dep => Bl._dal.Task.Read(dep.Id)!.StartDate!.Value.Add(Bl._dal.Task.Read(dep.Id)!.Duration ?? TimeSpan.Zero) > t.StartDate))
                    throw new BOInvalidUpdateException("start date is impossible");
            }
        }
        if (t.AllDependencies != null && t.AllDependencies.Count > 0)
        {
            if (!t.AllDependencies.All(dep => Bl._dal.Task.Read(dep.Id) != null))
                throw new BOInvalidUpdateException("invalid dependencies");
            if (!IsValidDep(t.Id, t.AllDependencies.Select(t => t.Id)))
                throw new BOInvalidUpdateException("circle dependency");
        }
        if (t.Engineer != null && (Bl._dal.Task.ReadAll(task => task.EngineerId == t.Engineer.Id).Where(ta=> ta!.CompleteDate==null).Count()>1)) throw new BOInvalidUpdateException("engineer is catched");
    }

    //function to check if dependency is correct(no circle)
    private bool IsValidDep(int taskId, IEnumerable<int>? allDep)
    {
        if (allDep?.Count() == 0)
            return true;
        if (allDep!.Contains(taskId))
            return false;
        return allDep!.All(depId =>
             IsValidDep(taskId, Bl._dal.Dependency
                 .ReadAll(d => d!.DependentTask == depId)
                 .Where(dep => dep!.DependensOnTask != null)
                 .Select(dep => (int)dep!.DependensOnTask!)
                 .ToList()));
    }

    //return the status of task
    private BO.TaskStatus CalcStatus(int id)
    {
        DO.Task myTask = Bl._dal.Task.Read(id)!;
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