using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        ITask Task { get; }
        IEngineer Engineer { get; }
        IDependency Dependency { get; }

        //begin & finish project dates
        DateTime? beginDate { get; set; }
        DateTime? finishDate { get; set; }
        public void Reset();
    }
}
