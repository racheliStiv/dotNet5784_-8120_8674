namespace PL;
using System.Collections.Generic;
using System;
using System.Collections;


internal class EngineerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> e_enums =
        (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => e_enums.GetEnumerator();
    
}
internal class TaskStatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.TaskStatus> e_enums =
    (Enum.GetValues(typeof(BO.TaskStatus)) as IEnumerable<BO.TaskStatus>)!;
    public IEnumerator GetEnumerator() => e_enums.GetEnumerator();
}


