namespace DO;
/// <summary>
/// A task entity - contains details about the task, the tasks that depend on it and the execution of the task
/// </summary>
/// <param name="Id"></param>
/// <param name="Alias"></param>
/// <param name="Description"></param>
/// <param name="CreatedAtDate"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="Duration"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="Product"></param>
/// <param name="Remarks"></param>
/// <param name="EngineerId"></param>
/// <param name="Complexity"></param>


public record Task
(
        int Id,
        string? Alias = null,
        string? Description = null,
        DateTime? CreatedAtDate = null,
        DateTime? StartDate = null,
        DateTime? ScheduledDate = null,
        TimeSpan? Duration = null,
        DateTime? DeadlineDate = null,
        DateTime? CompleteDate = null,
        string? Product = null,
        string? Remarks = null,
        int? EngineerId = null,
        DO.EngineerExperience? Complexity = null
    )

{
    public Task() : this(0) { }    //empty ctr
}




