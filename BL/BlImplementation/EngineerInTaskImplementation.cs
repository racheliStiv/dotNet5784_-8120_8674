using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerInTaskImplementation : IEngineerInTask
{
    public IEnumerable<EngineerInTask> GetAllEngInTask(Func<DO.Engineer, bool>? filter = null)
    {
        IEnumerable<DO.Engineer?> engineers = Bl._dal.Engineer.ReadAll(filter);
        var engineers_in_task = engineers.Select(eng => new EngineerInTask
        {
            Id = eng!.Id,
            Name = eng.Name ?? "",
        }).ToList();
        return engineers_in_task;
    }
    public EngineerInTask GetEngInTaskDetails(int id)
    {
        try
        {
            DO.Engineer? eng = (Bl._dal.Engineer.Read(id)) ?? throw new BO.BODoesNotExistException($"engineer with id = {id} is not exsist");
            return new EngineerInTask() { Id = eng.Id, Name = eng.Name };
        }
        catch (Exception ex)
        {
            throw new BO.BODoesNotExistException(ex.Message);
        }
    }
}