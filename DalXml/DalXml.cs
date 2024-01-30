using DalApi;
using DO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    sealed internal class DalXml : IDal
    {
        public static IDal Instance { get; } = new DalXml();
        private DalXml() { }

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
