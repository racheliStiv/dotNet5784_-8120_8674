using BlApi;
namespace BlImplementation;

internal class EngineerInTaskImplementation : IEngineerInTask
{
    private DalApi.IDal _dal = Factory.Get;

    public int GetEngOfTask(int task_id)
    {
        return _dal.Task.Read(task_id)?.EngineerId ?? 0;
    }
}
