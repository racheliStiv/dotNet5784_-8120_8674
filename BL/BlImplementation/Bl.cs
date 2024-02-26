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

        private DateTime startDate;

        public DateTime? StartDate
        {
            get { return Factory.Get.BeginDate; }
            set { Factory.Get.BeginDate = startDate; Status = ProjectStatus.IN; }
        }

        public void CreateSchedule()
        {
            Status = ProjectStatus.AFTER;
        }
    }
}
