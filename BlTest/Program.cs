using BlApi;
using BO;

namespace BlTest
{

    internal class Program
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static void Main_menu()
        {
            //main menu input
            Console.WriteLine("choose an entity u wana check \n 1 to Task\n 2 to Engineer \n 0 to exit \n");
            bool flag = true;
            while (flag)
            {
                flag = false;
                int choose = int.Parse(Console.ReadLine() ?? "");
                switch (choose)
                {
                    case 0:
                        return;
                    case 1:
                        Sub_menu("task");
                        break;
                    case 2:
                        Sub_menu("engineer");
                        break;
                    default:
                        Console.WriteLine("wrong input");
                        flag = true;
                        break;
                }
            }

        }
        public static void Sub_menu(string entity)
        {
            Console.WriteLine($"choose an act for {entity}, \n 0 to main menu \n 1 to Create\n 2 to Delete\n 3 to Update \n 4 to Get All Tasks \n 5 to Get Task Detail \n");
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
        public static void Create(string entity)
        {
            string format = "dd/hh/mm";
            switch (entity)
            {
                case "task":
                    {
                        Console.WriteLine($"insert:\n alias \n description \n planned start date \n duration \n  product \n remarks \n complexity(0-4)  \n\n in a date data insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? plannedStartDate = Console.ReadLine();
                        string? duration = Console.ReadLine();
                        string? product = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int complexityLevel = int.Parse(Console.ReadLine() ?? "");
                        string[] d_h_m = duration!.Split(':');
                        TimeSpan newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));
                        DateTime? readyPlannedStartDate = null;
                        if (plannedStartDate != null && plannedStartDate != "")
                        {
                            readyPlannedStartDate = DateTime.ParseExact(plannedStartDate!, format, null);
                        }

                        Task t = new()
                        {
                            Id = 0,
                            Description = description,
                            Alias = alias,
                            Status = BO.Status.UNSCHEDULED,
                            CreatedAtDate = DateTime.Now,
                            AllDependencies = null,
                            PlannedStartDate = readyPlannedStartDate,
                            StartDate = null,
                            PlannedFinishDate = null,
                            Remarks = remarks,
                            Product = product,
                            Duration = newTS,
                            Engineer = null,
                            ComplexityLevel = (BO.EngineerExperience)complexityLevel
                        };
                        s_bl.Task.Create(t);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert: \n id \n name \n email \n level(0-4)\n cost \n\n");
                        int? id = int.TryParse(Console.ReadLine(), out int res1) ? res1 : null;
                        while (id == null)
                        {
                            Console.WriteLine("must have ID for engineer");
                            id = int.TryParse(Console.ReadLine(), out int res2) ? res2 : null;
                        }
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        int? level = int.TryParse(Console.ReadLine(), out int res3) ? res3 : null;
                        EngineerExperience? engExp = (BO.EngineerExperience?)level ?? null;
                        double cost = double.Parse(Console.ReadLine() ?? "");
                        Engineer newEng = new()
                        {
                            Id = id.Value,
                            Name = name,
                            Email = email,
                            Level = engExp,
                            Cost = cost
                        };
                        try { s_bl.Engineer.Create(newEng); }
                        catch (BO.BOAlreadyExistsException ex)
                        { throw new BO.BOAlreadyExistsException(ex.Message); }
                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void Delete(string entity)
        {
            int? id;
            Console.WriteLine($"insert id of {entity} to delete");
            id = int.Parse(Console.ReadLine() ?? "");
            while (id == null)
            {
                Console.WriteLine("must receive ID");
                id = int.Parse(Console.ReadLine() ?? "");
            }
            switch (entity)
            {
                case "task":
                    {
                        try
                        {
                            s_bl.Task.Delete(id.Value);
                        }
                        catch (BO.BODeletionImpossibleException ex)
                        {
                            throw new BO.BODeletionImpossibleException(ex.Message);
                        }
                        catch (BO.BODoesNotExistException ex)
                        {
                            throw new BO.BODoesNotExistException(ex.Message);
                        }
                    }
                    break;
                case "engineer":
                    {
                        try
                        {
                            s_bl.Engineer.Delete(id.Value);
                        }
                        catch (BO.BODeletionImpossibleException ex)
                        {
                            throw new BO.BODeletionImpossibleException(ex.Message);
                        }
                        catch (BO.BODoesNotExistException ex)
                        {
                            throw new BO.BODoesNotExistException(ex.Message);
                        }
                    }
                    break;
            }
        }
        public static void Update(string entity)
        {
            int? id;
            Console.WriteLine($"insert id of {entity} to update");
            id = int.Parse(Console.ReadLine() ?? "");
            while (id == null)
            {
                Console.WriteLine("must receive ID");
                id = int.Parse(Console.ReadLine() ?? "");
            }
            string format = "dd/hh/mm";
            switch (entity)
            {
                case "task":
                    {
                        //if(ProjectStatus.status==A)
                        //{ }
                        Console.WriteLine($"insert:\n alias \n description \n planned start date \n duration \n  product \n remarks \n complexity(0-4)  \n\n in a date data insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? plannedStartDate = Console.ReadLine();
                        string? duration = Console.ReadLine();
                        string? product = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int complexityLevel = int.Parse(Console.ReadLine() ?? "");
                        string[] d_h_m = duration!.Split(':');
                        TimeSpan newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));
                        DateTime? readyPlannedStartDate = null;
                        if (plannedStartDate != null && plannedStartDate != "")
                        {
                            readyPlannedStartDate = DateTime.ParseExact(plannedStartDate!, format, null);
                        }

                        Task t = new()
                        {
                            Id = id.Value,
                            Description = description,
                            Alias = alias,
                            Status = BO.Status.UNSCHEDULED,
                            CreatedAtDate = DateTime.Now,
                            AllDependencies = null,
                            PlannedStartDate = readyPlannedStartDate,
                            StartDate = null,
                            PlannedFinishDate = null,
                            Remarks = remarks,
                            Product = product,
                            Duration = newTS,
                            Engineer = null,
                            ComplexityLevel = (BO.EngineerExperience)complexityLevel
                        };
                        s_bl.Task.Create(t);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert: \n name \n email \n level(0-4)\n cost \n\n");
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        int? level = int.TryParse(Console.ReadLine(), out int res3) ? res3 : null;
                        EngineerExperience? engExp = (BO.EngineerExperience?)level ?? null;
                        double cost = double.Parse(Console.ReadLine() ?? "");
                        Engineer newEng = new()
                        {
                            Id = id.Value,
                            Name = name,
                            Email = email,
                            Level = engExp,
                            Cost = cost
                        };
                        try { s_bl.Engineer.Create(newEng); }
                        catch (BO.BOAlreadyExistsException ex)
                        { throw new BO.BOAlreadyExistsException(ex.Message); }
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
                    try
                    {
                        Console.WriteLine(s_bl!.Task!.GetTaskDetails(id));
                    }
                    catch (BO.BODoesNotExistException ex)
                    {
                        throw new BO.BODoesNotExistException(ex.Message);
                    }
                    break;
                case "engineer":
                    try
                    {
                        Console.WriteLine(s_bl!.Engineer!.GetEngineerDetails(id));
                    }
                    catch (BO.BODoesNotExistException ex)
                    {
                        throw new BO.BODoesNotExistException(ex.Message);
                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void ReadAll(string entity) { }
        static void Main()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.DO();
            Main_menu();
        }
    }
}
