using System.Text.RegularExpressions;
using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    //get BO engineer and create new DO engineer
    public int Create(Engineer? boEngineer)
    {
        try
        {
            return _dal.Engineer.Create(BO_to_DO(boEngineer));
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BOAlreadyExistsException(ex.Message);
        }
        //לברר האם צריך להוסיף עוד קטצ
    }

    //get engineer id & delete its from DO lay
    public void Delete(int id)
    {
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
        catch (DO.DalDeletionImpossibleException ex)
        {
            throw new BO.BODeletionImpossibleException(ex.Message);
        }
    }

    //get all engineers from DO lay
    public IEnumerable<Engineer?> GetAllEngineers(Func<Engineer, bool>? filter = null)
    {
        //get all Engineers from DO
        IEnumerable<DO.Engineer?> do_engineer = _dal.Engineer.ReadAll();

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
            return DO_to_BO(_dal.Engineer.Read(id)) ?? throw new BO.BODoesNotExistException($"can't get engineer details of: ${id}");
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

            if (bo_engineer.Task != null)
            {
                if (IBl.Status != ProjectStatus.AFTER)
                    throw new BOInvalidUpdateException("invalid engineer update before AFTER");

                else
                {
                    DO.Engineer? origin_en = _dal.Engineer.Read(bo_engineer.Id);
                    if (origin_en == null)
                        throw new BODoesNotExistException("Engineer undefined in dal");
                    if()
                 
                        if (לא היו עוד משימות)
                        {

                        }

                        if (זה עדכון משימה) { }          
             
                }
            }
            _dal.Engineer.Update(BO_to_DO(bo_engineer!));
        }
        catch (BO.BODoesNotExistException ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new BO.BOInvalidUpdateException(ex.Message);
        }
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
        DO.Task? t = _dal.Task.Read(e => doEngineer.Id == e.Id);
        if (t != null)
            task = new TaskInEngineer() { Id = t!.Id, Alias = t?.Alias };
        else
            task = null;
        Engineer boEngineer = new Engineer() { Id = doEngineer.Id, Name = doEngineer.Name, Email = doEngineer.Email, Level = (BO.EngineerExperience?)doEngineer.Level, Cost = doEngineer.Cost, Task = task };
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
            throw new BO.BOCanNotBeNullException("missing engineer");
        if (e.Name == null || e.Email == null || e?.Level == null || e?.Cost == null)
            throw new BO.BOCanNotBeNullException("missing details for engineer");
        if (
        !ValidId(e.Id) ||
        !ValidEmail(e.Email) ||
        e.Name == "" ||
        e.Cost < 0)
            throw new BO.BOInvalidDetailsException("invalid details for engineer");
    }
}