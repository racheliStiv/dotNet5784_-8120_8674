using BlApi;
using BO;
using System.ComponentModel;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public static readonly DalApi.IDal _dal = Factory.Get;

        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IEngineerInTask EngineerIntask => new EngineerInTaskImplementation();

        public ITaskInEngineer TaskInEngineer => new TaskInEngineerImplementation();

        public ITaskInList TaskInList => new TaskInListImplementation();

        private static ProjectStatus status = _dal.Task.ReadAll().Count() > 0 && _dal.Task.ReadAll().Where(t => t != null).All(t => t!.ScheduledDate != null) ? ProjectStatus.AFTER : (Factory.Get.BeginDate.HasValue ? ProjectStatus.IN : ProjectStatus.BEFORE);

        public static ProjectStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        public DateTime? StartDate
        {
            get { return Factory.Get.BeginDate; }
            set { Factory.Get.BeginDate = value; status = ProjectStatus.IN; }
        }

        private static DateTime s_Clock = DateTime.Now.Date;
        public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
        public void AdvanceTimeByHour()
        {
            Clock = Clock.AddHours(1);
        }
        public void AdvanceTimeByDay()
        {
            Clock = Clock.AddDays(1);
        }
        public void InitializeTime()
        {
            Clock = DateTime.Now;
        }


    }
}
