using BlApi;
using BO;
using System.Linq;

namespace BlImplementation;

internal class TaskInListImplementation : ITaskInList
{

    //public IEnumerable<TaskInList?> GetAllTasksInList(Func<BO.Task, bool>? filter)
    //{
    //    IEnumerable<DO.Task?> do_tasks = Bl._dal.Task.ReadAll();
    //    IEnumerable<TaskInList?> bo_tasks = do_tasks
    //        .Select(do_task => new TaskInList
    //        {
    //            Id = do_task!.Id,
    //            Description = do_task!.Description,
    //            Alias = do_task!.Alias,
    //            Status = CalcStatus(do_task.Id),
    //        });

    //    return bo_tasks;
    //}
    private IEnumerable<BO.Task> ToBoTasks(IEnumerable<DO.Task?> do_tasks)
    {
        return do_tasks.Select(do_task => new BO.Task
        {
            Id = do_task!.Id,
            Description = do_task!.Description,
            Alias = do_task!.Alias,
        });
    }

    public IEnumerable<TaskInList?> GetAllTasksInList(Func<BO.Task, bool>? filter)
    {
        IEnumerable<DO.Task?> do_tasks = Bl._dal.Task.ReadAll();

        // Map DO tasks to BO tasks using extension method
        IEnumerable<BO.Task> bo_tasks = ToBoTasks(do_tasks);

        // Apply filtering using the provided function
        IEnumerable<BO.Task> filteredTasks = filter == null
            ? bo_tasks
            : bo_tasks.Where(filter);

        return filteredTasks.Select(bo_task => new TaskInList
        {
            Id = bo_task.Id,
            Description = bo_task.Description,
            Alias = bo_task.Alias,
            Status = CalcStatus(bo_task.Id),
        });
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
