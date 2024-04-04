namespace BO;

public class TaskInList
{
    public int Id { get; init; } 
    public string? Description { get; init; } 
    public string? Alias { get; init; } 
    public BO.TaskStatus Status { get; init; }
    public TaskInList(Task t)
    {
        Id = t.Id;
        Description = t.Description;
        Alias = t.Alias;
        Status = t.Status;
    }
    public TaskInList() { }
    public override string ToString() => this.ToStringProperty();
   
}
