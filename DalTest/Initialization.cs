namespace DalTest;
using Dal;
using DalApi;
using DO;
using System.Xml.Linq;

public static class Initialization
{
    private static IDal? s_dal;
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
            while (s_dal!.Engineer != null && s_dal!.Engineer.Read(id) != null);
            //Email initialization
            string email = name.Substring(0, name.IndexOf(' ')) + s_rand.Next(111, 999) + "@gmail.com";
            //Level initialization
            DO.EngineerExperience ex = (EngineerExperience)i++;
            //Cost initialization
            double cost = 50 + (int)ex * 80;
            Engineer newEng = new(id, name, email, ex, cost);
            //Adding to data list
            s_dal!.Engineer?.Create(newEng);
        }
    }
    //An operation that creates 20 tasks and initializes data in them using a random method
    private static void CreateTasks()
    {
        Engineer? en;
        List<Engineer?> allEngineers;
        //In case there isn't engineers, impossible create task
        if (s_dal!.Engineer == null)
            throw new DalCanNotBeNullException("There isn't any engineer in your data");
        //Loop that initialize the task's detail
        for (int i = 1; i <= 20; i++)
        {
            string alias, description = "";
            alias = "T" + i;
            description += (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122);
            //Length of time to perform a task
            TimeSpan requiredEffortTime =new TimeSpan(s_rand.Next(364), s_rand.Next(23), s_rand.Next(59), s_rand.Next(59));
            //What level of engineer is required
            DO.EngineerExperience complexity = (EngineerExperience)s_rand.Next(3);
            //Planned date for the start of work
            DateTime scheduleDate = DateTime.Now.AddDays(s_rand.Next(30));
            //When should it be finished?
            DateTime deadLine = (scheduleDate + requiredEffortTime).AddDays(i);
            //Final end date
            DateTime? completedDate = (i % 2 == 0) ? null : deadLine;
            string product = description + description;
            string remark = description.Substring(1, 2);
            allEngineers = s_dal!.Engineer.ReadAll().ToList();
            //Choosing an engineer to handle the task
            do
            {
                en = allEngineers[s_rand.Next(allEngineers.Count)];
            } while (en!.Level < complexity);
            //Creating a task and adding it to the task collection
            Task newTask = new(0, alias, description, DateTime.Now, scheduleDate, scheduleDate, requiredEffortTime, deadLine,  completedDate, product,  remark,  en.Id, complexity );
            s_dal!.Task?.Create(newTask);
        }

    }
    //An operation that creates 40 dependencies and initializes data in them using a random method
    private static void CreateDependencies()
    {
        //In case there isn't tasks, immposible creat a dependency
        if (s_dal!.Task == null)
            throw new DalCanNotBeNullException("There isn't any task in your data");
        //Create 36 dependencies. Each of the last two tasks creates a dependency on all the tasks that are defined before it
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                s_dal!.Dependency?.Create(new(0, s_dal!.Task!.Read(s_dal!.Task.ReadAll().ToList().Count - i)!.Id, s_dal!.Task!.ReadAll().ToList()[j]!.Id));

              //  s_dal!.Dependency?.Create(new(0, s_dal!.Task!.Read(s_dal!.Task.ReadAll().ToList().Count - i)!.Id, s_dal!.Task!.ReadAll().ToList()[j]!.Id));
            }
        }
        //4 last dependencies not created in the previous loop
        for (int i = 0; i < 4; i++)
        {
            s_dal!.Dependency?.Create(new(0, s_dal!.Task!.Read(s_dal!.Task.ReadAll().ToList().Count - 2)!.Id, s_dal!.Task!.ReadAll().ToList()[i]!.Id));
        }

    }
    //A function that triggers all creation and initialization of the entities
    public static void DO()
    {
        s_dal = Factory.Get; //stage 4

        //call to initialization methods
        CreateEngineers();
        CreateTasks();
        CreateDependencies();
    }
}
