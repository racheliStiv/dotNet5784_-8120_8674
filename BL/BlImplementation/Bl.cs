using BlApi;
using BO;
using System.ComponentModel;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IEngineerInTask EngineerIntask =>  new EngineerInTaskImplementation();

        public ITaskInEngineer TaskInEngineer =>  new TaskInEngineerImplementation();

        public ITaskInList TaskInList =>  new TaskInListImplementation();

        public ProjectStatus Status = ProjectStatus.A;

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; Status = ProjectStatus.B; }
        }

        //זה עם עושים מה
        ProjectStatus IBl.Status => throw new NotImplementedException();

        public void CreateSchedule()
        {
        }
    }
}
