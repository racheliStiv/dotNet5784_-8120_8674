namespace DalTest;
using DalApi;
using DO;
using System.Xml.Linq;

public static class Initialization
{
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;
    private static IEngineer? s_dalEngineer;

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
            while (s_dalEngineer!.Read(id) != null);

            string email = name.Substring(0, name.IndexOf(' ')) + s_rand.Next(111, 999) + "@gmail.com";

            DO.EngineerExperience ex = (EngineerExperience)s_rand.Next(0, 3);
            double cost = 50 + (int)ex * 80;

            Engineer newEng = new(id, name, email, ex, cost);

            s_dalEngineer!.Create(newEng);
        }

    }
    private static void createTasks()
    {
        foreach (var name in engNames)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer!.Read(id) != null);

            bool? b = name.Contains('i') ? true : false;

            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _bdt = start.AddDays(s_rand.Next(range));

            string? _alias = (id % 2) == 0 ? _name + "ALIAS" : null;

            Student newStu = new(id, _name, _alias, _b, _bdt);

            s_dalStudent!.Create(newStu);
        }
    }
    private static void createDependencies()
    {
        for (int i = 0; i < 40; i++)
        {
            List<Task> allTask = s_dalTask!.ReadAll();
        }    
    }
}
