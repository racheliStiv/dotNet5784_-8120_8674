namespace Dal;
using DO;
internal static class DataSource
{
    internal static class Config
    {
        /// <summary>
        /// Task running key
        /// </summary>
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        /// <summary>
        /// Dependency running key
        /// </summary>
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        //initialize the dates of begin & finish to be null
        internal static DateTime? beginDate = null;
        internal static DateTime? finishDate = null;

    }
    //entity lists definitions
    internal static List<Task> Tasks { get; } = new();
    internal static List<Engineer> Engineers { get; } = new();
    internal static List<Dependency> Dependencies { get; } = new();
    

}       
