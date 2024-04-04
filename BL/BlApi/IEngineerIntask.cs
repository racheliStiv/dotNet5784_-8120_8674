using BO;

namespace BlApi;

public interface IEngineerInTask 
{
    public IEnumerable<EngineerInTask> GetAllEngInTask(Func<DO.Engineer, bool>? filter = null);
    public EngineerInTask GetEngInTaskDetails(int id);
}
