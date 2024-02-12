using BO;
namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IEngineerIntask EngineerIntask { get; }
    public ITaskInEngineer TaskInEngineer { get; }
    public ITaskInList TaskInList { get; }
  //  private DateTime startDate;
    public DateTime StartDate 
    {
        get { return startDate; }
        set {  startDate = value; Status = ProjectStatus.B; } 
    }
    public ProjectStatus Status { get; } = ProjectStatus.A;
    public void CreateSchedule();


}
