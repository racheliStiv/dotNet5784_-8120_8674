using BlApi;
using BO;
using System.Linq;

namespace BlImplementation;

internal class TaskInListImplementation : ITaskInList
{ 
    public IEnumerable<TaskInList>? GetAllTasksInList(IEnumerable<BO.Task> tasks)
    {
        IEnumerable<TaskInList>? readyTasks = tasks.Where(task => task != null)
            .Select(do_task => new TaskInList
            {
                Id = do_task!.Id,
                Description = do_task!.Description,
                Alias = do_task!.Alias,
                Status = CalcStatus(do_task.Id),
            });
        return readyTasks;
    }


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
