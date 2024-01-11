using DalApi;
using DO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    sealed public class DalXml : IDal
    {
        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IDependency Dependency => new DependencyImplementation();

        //return & update the project dates
        public DateTime? beginDate { get; set; }
        public DateTime? finishDate { get; set; }


        //clear all data
        public void Reset()
        {
            Dependency.Reset();
            Task.Reset();
            Engineer.Reset();
        }
    }
}
