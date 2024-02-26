using DalApi;
using DO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public DateTime? BeginDate
        {
            get {
                XElement root = XMLTools.LoadListFromXMLElement("data-config");
                return root.ToDateTimeNullable("startDate");
            }
            set {
                XElement root = XMLTools.LoadListFromXMLElement("data-config");
                root.Element("startDate")?.SetValue(value!.Value.ToString("dd-mm-yy"));
                XMLTools.SaveListToXMLElement(root, "data-config");
            }
        }
        public DateTime? FinishDate { get; set; }


        //clear all data
        public void Reset()
        {
            Dependency.Reset();
            Task.Reset();
            Engineer.Reset();
        }
    }
}
