﻿using DalApi;


namespace Dal
{
    sealed public class DalList : IDal
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
            DataSource.Dependencies.Clear();
            DataSource.Tasks.Clear();
            DataSource.Engineers.Clear();
        }
    }
}
