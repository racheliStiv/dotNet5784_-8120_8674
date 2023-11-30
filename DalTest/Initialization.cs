﻿namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;

public static class Initialization
{
    //Creating a link object between layers, for each entity
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;
    private static IEngineer? s_dalEngineer;

    private static readonly Random s_rand = new();

    //An operation that creates 5 engineers and initializes data in them using a random method
    private static void CreateEngineers()
    {
        const int MIN_ID = 200000000;
        const int MAX_ID = 400000000;
        //Array of the engineers name
        string[] engNames = { "Eyal Gilboa", "Dan Chalutz", "Asaf Yalensky", "Orit Vladi", "Racheli stav" };
        int i = 0;
        //Loop that initialize the engineer's detail
        foreach (var name in engNames)
        {
            int id;
            //Id initialization
            do id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer != null && s_dalEngineer.Read(id) != null);
            //Email initialization
            string email = name.Substring(0, name.IndexOf(' ')) + s_rand.Next(111, 999) + "@gmail.com";
            //Level initialization
            DO.EngineerExperience ex = (EngineerExperience)i++;
            //Cost initialization
            double cost = 50 + (int)ex * 80;
            Engineer newEng = new(id, name, email, ex, cost);
            //Adding to data list
            s_dalEngineer?.Create(newEng);
        }

    }
    //An operation that creates 20 tasks and initializes data in them using a random method
    private static void CreateTasks()
    {
        Engineer en;
        List<Engineer> allEngineers;
        //In case there isn't engineers, impossible create task
        if (s_dalEngineer == null)
            throw new Exception("There isn't any engineer in your data");
        //Loop that initialize the task's detail
        for (int i = 1; i <= 20; i++)
        {
            string alias, description = "";
            alias = "T" + i;
            description += (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122);
            //Length of time to perform a task
            TimeSpan requiredEffortTime =new TimeSpan(s_rand.Next(364), s_rand.Next(23), s_rand.Next(59), s_rand.Next(59));
            bool isMilestone = description.Contains('i') ? true : false;
            //What level of engineer is required
            DO.EngineerExperience complexity = (EngineerExperience)s_rand.Next(3);
            //Planned date for the start of work
            DateTime scheduleDate = DateTime.Now.AddDays(s_rand.Next(30));
            //When should it be finished?
            DateTime deadLine = (scheduleDate + requiredEffortTime).AddDays(i);
            //Final end date
            DateTime? completedDate = (i % 2 == 0) ? null : deadLine;
            string deliveryable = description + description;
            string remark = description.Substring(1, 2);
            allEngineers = s_dalEngineer.ReadAll();
            //Choosing an engineer to handle the task
            do
            {
                en = allEngineers[s_rand.Next(allEngineers.Count)];
            } while (en.Level < complexity);
            //Creating a task and adding it to the task collection
            Task newTask = new(0, alias, description, DateTime.Now, requiredEffortTime, isMilestone, complexity, scheduleDate, scheduleDate, deadLine, completedDate, deliveryable, remark, en.Id);
            s_dalTask?.Create(newTask);
        }

    }
    //An operation that creates 40 dependencies and initializes data in them using a random method
    private static void CreateDependencies()
    {
        //In case there isn't tasks, immposible creat a dependency
        if (s_dalTask == null)
            throw new Exception("There isn't any task in your data");
        //Creat 36 dependencies. Each of the last two tasks creates a dependency on all the tasks that are defined before it
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                s_dalDependency?.Create(new(0, s_dalTask!.Read(s_dalTask.ReadAll().Count - i)!.Id, s_dalTask!.ReadAll()[j].Id));
            }
        }
        //4 last dependencies not created in the previous loop
        for (int i = 0; i < 4; i++)
        {
            s_dalDependency?.Create(new(0, s_dalTask!.Read(s_dalTask.ReadAll().Count - 2)!.Id, s_dalTask!.ReadAll()[i].Id));
        }
    }
    //A function that triggers all creation and initialization of the entities
    public static void DO(IDependency? dalDependency, ITask? dalTask, IEngineer? dalEngineer)
    {
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        CreateEngineers();
        CreateTasks();
        CreateDependencies();
    }
}
