using System.Text.RegularExpressions;
using BlApi;
using BO;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    TaskImplementation task_imp =new TaskImplementation();

    //get BO engineer and create new DO engineer
    public int Create(Engineer? boEngineer)
    {
        try
        {
            return Bl._dal.Engineer.Create(BO_to_DO(boEngineer));
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BOAlreadyExistsException(ex.Message);
        }
    }

    //get engineer id & delete its from DO lay
    public void Delete(int id)
    {
        try
        {
            if ((Bl._dal.Task.Read(t => t.EngineerId == id && t.CompleteDate != null)) != null)
                throw new BODeletionImpossibleException("delete is immpossible");
            Bl._dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
        catch (BODeletionImpossibleException ex)
        {
            throw new BODeletionImpossibleException(ex.Message);
        }
    }

    //get all engineers from DO lay
    public IEnumerable<Engineer?> GetAllEngineers(Func<Engineer, bool>? filter)
    {
        //get all Engineers from DO
        IEnumerable<DO.Engineer?> do_engineer = Bl._dal.Engineer.ReadAll();

        //change each Engineer to BO
        IEnumerable<Engineer?> bo_engineers = do_engineer.Where(e => e != null).Select(e => DO_to_BO(e!));

        //checks which engineers meet the filter
        if (filter != null)
        {
            bo_engineers = bo_engineers.Where(filter!);
        }
        return bo_engineers;
    }

    //get BO angineer by id
    public Engineer GetEngineerDetails(int id)
    {
        try
        {
            return DO_to_BO(Bl._dal.Engineer.Read(id)) ?? throw new BO.BODoesNotExistException($"engineer with id = {id} is not exsist");
        }
        catch (Exception ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
    }

    //get BO engineer and update the DO engineer
    public void Update(Engineer? bo_engineer)
    {
        try
        {
            if (bo_engineer == null)
                throw new BONullObj("didn't get an engineer to update");
            DO.Engineer origin_en = Bl._dal.Engineer.Read(bo_engineer.Id) ?? throw new BODoesNotExistException("Engineer undefined in dal");
            if ((DO.EngineerExperience)bo_engineer.Level! < origin_en.Level)
                throw new BOInvalidUpdateException("can't update level of engineer to lower");
            if (bo_engineer.Task != null)
            {               
                if (Bl.Status != ProjectStatus.AFTER)
                    throw new BOInvalidUpdateException("can't update. before AFTER create luz");
                if (bo_engineer.Task.Id != GetTaskOfEng(origin_en.Id) && Bl._dal.Task.Read(bo_engineer.Task.Id) == null)
                    throw new BOInvalidUpdateException("this task is not exist");
                if ((Bl._dal.Task.Read(bo_engineer.Task.Id)!.CompleteDate != null))
                    throw new BOTaskIsDone("this task is done");
                if (GetEngOfTask(bo_engineer.Task!.Id) != 0)
                    throw new BOTaskAlreadyOccupied("this task already caught");
                IEnumerable<DO.Dependency> dependencies = Bl._dal!.Dependency.ReadAll(t => t!.DependentTask == bo_engineer.Task.Id)!;
                if (!IsDepDone(dependencies) || (EngineerExperience)Bl._dal.Task.Read(bo_engineer.Task.Id)!.Complexity! > bo_engineer.Level)
                    throw new BOTaskAlreadyOccupied("unable to update task in engineer");
                if (GetTaskOfEng(bo_engineer.Id) != 0)
                    throw new BOInvalidUpdateException("unable to update, in middle other task");
               
                BO.Task t =task_imp.GetTaskDetails(bo_engineer.Task.Id);
                EngineerInTask eng_of_task = new EngineerInTask(){ Id = bo_engineer.Id, Name = bo_engineer.Name};
                t.Engineer = eng_of_task;
                task_imp.Update(t);
                    }
            Bl._dal.Engineer.Update(BO_to_DO(bo_engineer!));
            
        }
        catch (BODoesNotExistException ex)
        {
            throw new BODoesNotExistException(ex.Message);
        }
        catch (BOInvalidUpdateException ex)
        {
            throw new BOInvalidUpdateException(ex.Message);
        }
    }

    //function to check if all dependencies of task were done
    private bool IsDepDone(IEnumerable<DO.Dependency> allD)
    {
        try
        {
            return allD.All(t => Bl._dal.Task.Read(t.DependensOnTask!.Value)!.CompleteDate != null);
        }
        catch (BOCanNotBeNullException ex)
        {
            throw new BOCanNotBeNullException(ex.Message + "not found task of dependency");
        }
    }
   
    //function that return engineer in task
    private int GetEngOfTask(int task_id)
    {
        return Bl._dal.Task.Read(task_id)?.EngineerId ?? 0;
    }

    //function that return task in engineer
    private int GetTaskOfEng(int eng_id)
    {
        return Bl._dal.Task.Read(t => t.EngineerId == eng_id && t.CompleteDate != null)?.Id ?? 0;
    }

    //change from BO engineer to DO engineer
    private DO.Engineer BO_to_DO(Engineer? boEngineer)
    {
        try
        {
            ValidBOEngineer(boEngineer);
            DO.Engineer doEngineer = new DO.Engineer(boEngineer!.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience?)boEngineer.Level, boEngineer.Cost);
            return doEngineer;
        }
        catch (BO.BOInvalidDetailsException ex)
        {
            throw new BO.BOInvalidDetailsException(ex.Message);
        }
    }

    //change from DO engineer to BO engineer
    internal Engineer? DO_to_BO(DO.Engineer? doEngineer)
    {
        if (doEngineer == null) return null;
        TaskInEngineer? task;
        DO.Task? t = Bl._dal.Task.Read(e => doEngineer.Id == e.EngineerId && e.CompleteDate == null);
        if (t != null)
            task = new TaskInEngineer() { Id = t!.Id, Alias = t?.Alias };
        else
            task = null;
        Engineer boEngineer = new Engineer() { Id = doEngineer.Id, Name = doEngineer.Name, Email = doEngineer.Email, Level = (EngineerExperience?)doEngineer.Level, Cost = doEngineer.Cost, Task = task };
        return boEngineer;
    }

    //function to check id validation
    private static bool ValidId(int Id)
    {
        string id = Id.ToString();
        string compare = "123456789";
        if (id.StartsWith("8"))
        {
            if (id.StartsWith("91"))
            {
                if (Regex.IsMatch(id, @"^[a-zA-Z]+$"))
                {
                    if (id.Length > compare.Length)
                    {
                        if (id.Length < compare.Length)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    //function to check email validation
    private static bool ValidEmail(string? email)
    {
        var trimmedEmail = email!.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    //function to check engineer validation
    private static void ValidBOEngineer(Engineer? e)
    {
        if (e == null)
            throw new BOCanNotBeNullException("missing engineer");
        if (e.Name == null || e.Email == null || e.Level == null || e?.Cost == null)
            throw new BO.BOCanNotBeNullException("missing details for engineer");
        if (
        !ValidId(e.Id) ||
        !ValidEmail(e.Email) ||
        e.Name == "" ||
        e.Cost < 0|| 
        (int)e.Level > 4 ||
        e.Level < 0)
            throw new BOInvalidDetailsException("invalid details for engineer");
    }
}