
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public BO.Status Status { get; set; }

    //mileston??
    public DateTime BaseLineStartDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ScheduledStartDate { get; set; }
    public DateTime ForecastDate { get; set; }
    public DateTime DeadlineDate { get; set; }
    public DateTime ComletedDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public Engineer? Engineer { get; set; }
    public BO.EngineerExperience ComplexityLevel { get; set; }

}
