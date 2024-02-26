using BO;
namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IEngineerInTask EngineerIntask { get; }
    public ITaskInEngineer TaskInEngineer { get; }
    public ITaskInList TaskInList { get; }
    public static DateTime? StartDate { get; set; }
    public  void CreateSchedule(); 
    public static ProjectStatus Status { get; }
}
