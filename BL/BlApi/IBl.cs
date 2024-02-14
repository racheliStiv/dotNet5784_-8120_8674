using BO;
namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IEngineerInTask EngineerIntask { get; }
    public ITaskInEngineer TaskInEngineer { get; }
    public ITaskInList TaskInList { get; }
    public DateTime StartDate { get; set; }
    public void CreateSchedule(); 
    public BO.ProjectStatus Status { get; }
}
