namespace DO;
/// <summary>
/// A task entity - contains details about the task, the tasks that depend on it and the execution of the task
/// </summary>
/// <param name="Id"></param>
/// <param name="Alias"></param>
/// <param name="Description"></param>
/// <param name="CreatedAtDate"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="IsMilestone"></param>
/// <param name="Complexity"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
/// <param name="EngineerId"></param>
public record Task
(
        int Id,
        string? Alias = null,
        string? Description = null,
        DateTime? CreatedAtDate = null,
        TimeSpan? RequiredEffortTime = null,
        bool IsMilestone = false,
        DO.EngineerExperience? Complexity = null,
        DateTime? StartDate = null,
        DateTime? ScheduledDate = null,
        DateTime? DeadlineDate = null,
        DateTime? CompleteDate = null,
        string? Deliverables = null,
        string? Remarks = null,
        int? EngineerId = null  )

{
    public Task() : this(0) { }    //empty ctr

}




