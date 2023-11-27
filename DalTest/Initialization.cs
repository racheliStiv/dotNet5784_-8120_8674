namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;

public static class Initialization
{
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;
    private static IEngineer? s_dalEngineer;
    static ITask? dalTask;
    static IDependency? dalDependency;
    static IEngineer? dalEngineer;

    private static readonly Random s_rand = new();

    private static void createEngineers()
    {
        const int MIN_ID = 200000000;
        const int MAX_ID = 400000000;
        string[] engNames = { "Eyal Gilboa", "Dan Chalutz", "Asaf Yalensky", "Orit Vladi", "Racheli stav" };
        foreach (var name in engNames)
        {
            int id;
            do

                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer != null && s_dalEngineer.Read(id) != null);


            string email = name.Substring(0, name.IndexOf(' ')) + s_rand.Next(111, 999) + "@gmail.com";

            int i = 0;
            DO.EngineerExperience ex = (EngineerExperience)i++;
            double cost = 50 + (int)ex * 80;

            Engineer newEng = new(id, name, email, ex, cost);

            s_dalEngineer?.Create(newEng);
        }

    }
    private static void createTasks()
    {
        Engineer en;
        List<Engineer> allEngineers;
        if (s_dalEngineer == null)
        {
            throw new Exception("There isn't any engineer in your data");
        }
        for (int i = 1; i <= 20; i++)
        {
            string alias, description = "";
            alias = "T" + i;
            description += (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122) + (char)s_rand.Next(97, 122);
            //אורך המשימה
            TimeSpan requiredEffortTime = new TimeSpan(s_rand.Next(364), s_rand.Next(23), s_rand.Next(59), s_rand.Next(59));
            bool isMilestone = description.Contains('i') ? true : false;
            DO.EngineerExperience complexity = (EngineerExperience)s_rand.Next(3);
            //תאריך מתוכנן לתחילת עבודה
            DateTime scheduleDate = DateTime.Now.AddDays(s_rand.Next(30));
            //עד מתי צריך לגמור
            DateTime deadLine = (scheduleDate + requiredEffortTime).AddDays(i);
            //תאריך סיום סופי
            DateTime? completedDate = (i % 2 == 0) ? null : deadLine;
            string deliveryable = description + description;
            string remark = description.Substring(2, 5);
            do
            {
                allEngineers = s_dalEngineer.ReadAll();
                en = allEngineers[s_rand.Next(allEngineers.Count)];
            } while (en.level < complexity);
            Task newTask = new(0, alias, description, DateTime.Now, requiredEffortTime, isMilestone, complexity, scheduleDate, scheduleDate, deadLine, completedDate, deliveryable, remark, en.Id);
            s_dalTask?.Create(newTask);
        }

    }
    private static void createDependencies()
    {
        Dependency dep;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                s_dalDependency?.Create((dep = new(0, s_dalTask!.ReadAll()[s_dalTask.ReadAll().Count - i].Id, s_dalTask!.ReadAll()[j].Id)));
            }
        }
        for (int i = 0; i < 4; i++)
        {
            s_dalDependency?.Create((dep = new(0, s_dalTask!.ReadAll()[s_dalTask.ReadAll().Count - 2].Id, s_dalTask!.ReadAll()[i].Id)));
        }
    }
    public static void DO(IDependency? dalDependency, ITask? dalTask, IEngineer? dalEngineer)
    {
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        createDependencies();
        createEngineers();
        createTasks();
    }
}
