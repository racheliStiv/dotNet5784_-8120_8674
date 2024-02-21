using BlApi;
using BO;
using System.ComponentModel;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IEngineerInTask EngineerIntask => new EngineerInTaskImplementation();

        public ITaskInEngineer TaskInEngineer => new TaskInEngineerImplementation();

        public ITaskInList TaskInList => new TaskInListImplementation();

        public ProjectStatus Status = ProjectStatus.BEFORE;
        
        //public ProjectStatus? Status
        //{
        //    get { return Status; }
        //    set { Status = (Status == null) ? ProjectStatus.BEFORE : value; }
        //}

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; Status = ProjectStatus.IN; }
        }

        //זה עם עושים מה
        //ProjectStatus IBl.Status => throw new NotImplementedException();
        public void CreateSchedule()
        {
            Status = ProjectStatus.AFTER;
        }
    }
}
