using BlApi;
using BO;
using DO;

namespace BlImplementation;

internal class TaskInEngineerImplementation : ITaskInEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public int GetTaskOfEng(int eng_id)
    {
        return _dal.Task.Read(t => t.EngineerId != null && t.EngineerId == eng_id)?.Id ?? 0;
    }
}
