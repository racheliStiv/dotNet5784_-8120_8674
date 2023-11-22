namespace DO;

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
    public Task() : this(0) { }
}




