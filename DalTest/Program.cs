using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {
        //stage 2
      //  static readonly IDal s_dal = new DalList();

        //stage 3
        static readonly IDal s_dal = new Dal.DalXml();

        //The main loop that invokes the selection of the desired entity
        public static void Main_menu()
        {

            //main menu input
            Console.WriteLine("choose an entity u wana check \n 1 to Task\n 2 to Engineer\n 3 to Dependency \n 0 to exit \n");
            int choose = int.Parse(Console.ReadLine() ?? "");
            switch (choose)
            {
                case 0: return;
                case 1:
                    Sub_menu("task");
                    break;
                case 2:
                    Sub_menu("engineer");
                    break;
                case 3:
                    Sub_menu("dependency");
                    break;
            }

        }

        //A secondary loop that invokes the selection of the desired action for the selected entity
        public static void Sub_menu(string entity)
        {
            Console.WriteLine($"choose an act for {entity}, one of 'crud'\n 0 to main menu \n 1 to Create\n 2 to Read\n 3 to ReadAll \n 4 to Update \n 5 to Delete \n");
            int choose = int.Parse(Console.ReadLine() ?? "");
            switch (choose)
            {
                case 0:
                    Main_menu();
                    break;
                case 1:
                    Create(entity);
                    break;
                case 2:
                    Read(entity);
                    break;
                case 3:
                    ReadAll(entity);
                    break;
                case 4:
                    Update(entity);
                    break;
                case 5:
                    Delete(entity);
                    break;
            }
        }


        //The CRUD operations that invoke the CRUD written in the DALAPI layer
        public static void Create(string entity)
        {
            string format = "dd/hh/mm";
            switch (entity)
            {
                case "task":
                    {
                        Console.WriteLine($"insert:\n alias \n description \n requierd effort time \n is milestone \n complexity(0-4) \n start date \n scheduled date \n deadline date \n complete date \n deliveryables \n remarks \n engineer id \n\n in a data date insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? requierd_effort_time = Console.ReadLine();
                        string[] d_h_m = requierd_effort_time!.Split('/');
                        string? milestone = Console.ReadLine();
                        int complexity = int.Parse(Console.ReadLine() ?? "");
                        string? start_date = Console.ReadLine();
                        string? scheduled_date = Console.ReadLine();
                        string? deadline_date = Console.ReadLine();
                        string? complete_date = Console.ReadLine();
                        string? deliveryables = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int engineer_id = int.Parse(Console.ReadLine() ?? "");
                        DO.Task t = new(0, alias, description, DateTime.Now, new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2]), 0), (milestone == "t"), (EngineerExperience)complexity, DateTime.ParseExact(start_date!, format, null), DateTime.ParseExact(scheduled_date!, format, null), DateTime.ParseExact(deadline_date!, format, null), DateTime.ParseExact(complete_date!, format, null), deliveryables, remarks, engineer_id);
                        s_dal!.Task?.Create(t);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert: \n id \n name \n email \n level(0-4)\ncost");
                        int id = int.Parse(Console.ReadLine() ?? "");
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        int level = int.Parse(Console.ReadLine() ?? "");
                        double cost = double.Parse(Console.ReadLine() ?? "");
                        Engineer newEng = new(id, name, email, (EngineerExperience)level, cost);
                        s_dal!.Engineer?.Create(newEng);

                    }
                    break;

                case "dependency":
                    {
                        Console.WriteLine("insert: \n dependent task \nDependens on task ");
                        int dependentTask = int.Parse(Console.ReadLine() ?? "");
                        int DependensOnTask = int.Parse(Console.ReadLine() ?? "");
                        Dependency newDep = new(0, dependentTask, DependensOnTask);
                        s_dal!.Dependency?.Create(newDep);
                    }
                    break;
            }
            Sub_menu(entity);
        }

        public static void Delete(string entity)
        {
            int id;
            switch (entity)
            {
                case "task":
                    {
                        Console.WriteLine("insert id of task");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dal!.Task?.Delete(id);
                    }
                    break;
                case "dependency":
                    {
                        Console.WriteLine("insert id of dependency");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dal!.Dependency?.Delete(id);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert id of engineer");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dal!.Engineer?.Delete(id);
                    }
                    break;
            }
            Sub_menu(entity);
        }

        public static void Read(string entity)
        {
            Console.WriteLine($"insert id of {entity} to print \n");
            int id = int.Parse(Console.ReadLine() ?? "");
            switch (entity)
            {
                case "task":
                    Console.WriteLine(s_dal!.Task!.Read(id));
                    break;
                case "dependency":
                    Console.WriteLine(s_dal!.Dependency!.Read(id));
                    break;
                case "engineer":
                    Console.WriteLine(s_dal!.Engineer!.Read(id));
                    break;
            }
            Sub_menu(entity);
        }

        public static void ReadAll(string entity)
        {
            switch (entity)
            {
                case "task":
                    foreach (var item in s_dal!.Task!.ReadAll())
                        Console.WriteLine(item);

                    break;
                case "dependency":
                    foreach (var item in s_dal!.Dependency!.ReadAll())
                        Console.WriteLine(item);
                    break;
                case "engineer":
                    foreach (var item in s_dal!.Engineer!.ReadAll())
                        Console.WriteLine(item);
                    break;
            }
            Sub_menu(entity);
        }

        public static void Update(string entity)
        {
            int id;
            string format = "dd/hh/mm";
            switch (entity)
            {
                case "task":
                    {
                        Console.WriteLine("insert id of task to update");
                        id = int.Parse(Console.ReadLine() ?? "");
                        Console.WriteLine($"insert:\n alias \n description \n requierd effort time \n is milestone \n complexity(0-4) \n start date \n scheduled date \n deadline date \n complete date \n deliveryables \n remarks \n engineer id \n\n in a data date insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? requierd_effort_time = Console.ReadLine();
                        string[] d_h_m = requierd_effort_time!.Split('/');
                        string? milestone = Console.ReadLine();
                        int? complexity = int.TryParse(Console.ReadLine(), out var res1) ? res1 : null;
                        string? start_date = Console.ReadLine();
                        string? scheduled_date = Console.ReadLine();
                        string? deadline_date = Console.ReadLine();
                        string? complete_date = Console.ReadLine();
                        string? deliveryables = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int? engineer_id = int.TryParse(Console.ReadLine(), out var result) ? result : null;
                        TimeSpan? newTS;
                        if (requierd_effort_time == "//" || requierd_effort_time == "")
                            newTS = s_dal!.Task!.Read(id)!.RequiredEffortTime;
                        else newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));
                        DO.Task original_t = s_dal!.Task!.Read(id)!;
                        DO.Task t = new(id, (alias == "") ? original_t.Alias : alias, (description == "") ? original_t.Description : description,
                            original_t.CreatedAtDate, newTS, (milestone != "") ? (milestone == "t") : original_t.IsMilestone,
                            complexity != null ? (EngineerExperience)complexity! : original_t.Complexity,
                            (start_date == "//" || start_date == "") ? original_t.StartDate : DateTime.ParseExact(start_date!, format, null),
                            (scheduled_date == "//" || scheduled_date == "") ? original_t.ScheduledDate : DateTime.ParseExact(scheduled_date!, format, null),
                            (deadline_date == "//" || deadline_date == "") ? original_t.DeadlineDate : DateTime.ParseExact(deadline_date!, format, null),
                            (complete_date == "//" || complete_date == "") ? original_t.CompleteDate : DateTime.ParseExact(complete_date!, format, null),
                            (deliveryables == "") ? original_t.Deliverables : deliveryables,
                            (remarks == "") ? original_t.Remarks : remarks,
                            engineer_id ?? original_t.EngineerId);
                        s_dal!.Task?.Update(t);
                    }
                    break;
                case "dependency":
                    {
                        Console.WriteLine("insert id of dependency to update");
                        id = int.Parse(Console.ReadLine() ?? "");
                        Dependency original_d = s_dal!.Dependency!.Read(id)!;
                        Console.WriteLine("insert: \n dependent task \ndependens on task");
                        int? dependentTask = int.TryParse(Console.ReadLine(), out var res1) ? res1 : null;
                        int? dependensOnTask = int.TryParse(Console.ReadLine(), out var res2) ? res2 : null;
                        Dependency newDep = new(id, dependentTask ?? original_d.DependentTask, dependensOnTask ?? original_d.DependensOnTask);
                        s_dal!.Dependency?.Update(newDep);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert id of engineer to update");
                        id = int.Parse(Console.ReadLine() ?? "");
                        Engineer original_e = s_dal!.Engineer!.Read(id)!;
                        Console.WriteLine("insert: \n  name \n email \n level(0-4)\ncost");
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        int? level = int.TryParse(Console.ReadLine(), out var res1) ? res1 : null;
                        double? cost = double.TryParse(Console.ReadLine(), out var res2) ? res2 : null;
                        Engineer newEng = new(id, name == "" ? original_e.Name : name, email == "" ? original_e.Email : email, level != null ? (EngineerExperience)level! : original_e.Level, cost ?? original_e.Cost);
                        s_dal!.Engineer?.Update(newEng);

                    }
                    break;
            }
            Sub_menu(entity);
        }

        //main
        public static void Main()
        {
            try
            {
                //reset suggest
                Console.WriteLine("do you want to initialize the data? (y / n)");
                char ch;
                bool choose;
                choose = Char.TryParse(Console.ReadLine(), out ch);
                if (ch == 'y')
                {
                    Console.WriteLine("are you sure about this? (y / n)");
                    choose = Char.TryParse(Console.ReadLine(), out ch);
                    if (ch == 'y')
                    {
                        s_dal.Reset();
                        Initialization.DO(s_dal);
                    }
                }

                Main_menu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }


}

