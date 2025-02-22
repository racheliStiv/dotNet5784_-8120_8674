﻿using DalApi;

namespace Dal
{
    sealed internal class DalList : IDal
    {
        public static IDal Instance { get; } = new DalList();
        private DalList() { }

        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IDependency Dependency => new DependencyImplementation();

        //return & update the project dates
        public DateTime? BeginDate { get; set; }
        public DateTime? FinishDate { get; set; }


        //clear all data
        public void Reset()
        {
            DataSource.Dependencies.Clear();
            DataSource.Tasks.Clear();
            DataSource.Engineers.Clear();
            BeginDate = null;
        }
    }
}
