public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public BO.Status Status { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public List<TaskInList>? AllDependencies { get; set; }    
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? PlannedFinishDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Product { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public BO.EngineerExperience? ComplexityLevel { get; set; }

}
