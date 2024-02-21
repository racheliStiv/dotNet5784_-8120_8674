using BlApi;
using BO;

namespace BlTest
{
    internal class Program
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        static bool IsDate(string date)
        {
            // בדיקה אם המחרוזת מכילה שני סלשים ותוכן של מספרים
            if (date.Split('/').Length == 3 && date.Replace("/", "").All(char.IsDigit))
            {
                // בדיקה אם הסלשים נמצאים במקומות הנכונים
                if (date[2] == '/' && date[5] == '/')
                {
                    // בדיקה אם המחרוזת תואמת את התבנית
                    if (System.Text.RegularExpressions.Regex.IsMatch(date, @"^\d{2}/\d{2}/\d{2}$"))
                    {
                        // חלוקת המחרוזת לשלושה חלקים על פי הסלש
                        string[] parts = date.Split('/');

                        // וידוא כי כל חלק הוא מספר שלם ותואם לטווח המותר
                        if (0 <= int.Parse(parts[0]) && int.Parse(parts[0]) <= 31 &&
                            0 <= int.Parse(parts[1]) && int.Parse(parts[1]) <= 12 &&
                            0 <= int.Parse(parts[2]) && int.Parse(parts[2]) <= 99)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static void Main_menu()
        {
            //main menu input
            Console.WriteLine("choose an entity u wana check \n 1 to Task\n 2 to Engineer \n 3 to update start date of groject \n 0 to exit \n");
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
                    //case 3:
                    //    Factory.Get.c
                    default:
                        Console.WriteLine("wrong input");
                        flag = true;
                        break;
                }
            }
        }
        public static void Sub_menu(string entity)
        {
            Console.WriteLine($"choose an act for {entity}, \n 0 to main menu \n 1 to Create\n 2 to Read\n 3 to Get All Tasks \n 4  to Update\n 5 to Delete \n");
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
                    try
                    {
                        Console.WriteLine($"insert:\n alias \n description \n  duration \n  product \n remarks \n complexity(0-4)  \n\n in a date data insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? duration = Console.ReadLine();
                        string? product = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int complexityLevel = int.Parse(Console.ReadLine() ?? "");
                        string[] d_h_m;
                        d_h_m = duration!.Split(':');
                        TimeSpan newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));

                        Task t = new()
                        {
                            Id = 0,
                            Description = description,
                            Alias = alias,
                            Status = BO.TaskStatus.UNSCHEDULED,
                            CreatedAtDate = DateTime.Now,
                            AllDependencies = null,
                            PlannedStartDate = null,
                            StartDate = null,
                            PlannedFinishDate = null,
                            Remarks = remarks,
                            Product = product,
                            Duration = newTS,
                            Engineer = null,
                            ComplexityLevel = (EngineerExperience)complexityLevel
                        };
                        try
                        {
                            s_bl.Task.Create(t);
                        }
                        catch (BOCannotAddNewOne ex)
                        {

                            throw new BOCannotAddNewOne(ex.Message);
                        }
                    }
                    catch (Exception)
                    {

                        throw new Exception("invalid input");
                    }
                    break;
                case "engineer":
                    {
                        try
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
                            EngineerExperience? engExp = (EngineerExperience?)level ?? null;
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
                            catch (BOAlreadyExistsException ex)
                            { throw new BOAlreadyExistsException(ex.Message); }
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message);
                        }

                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void Delete(string entity)
        {
            int? id;
            Console.WriteLine($"insert id of {entity} to delete");
            id = int.TryParse(Console.ReadLine(), out int Id1) ? Id1 : null;
            while (id == null)
            {
                Console.WriteLine("must receive ID");
                id = int.TryParse(Console.ReadLine(), out int Id2) ? Id2 : null;
            }
            switch (entity)
            {
                case "task":
                    try
                    {
                        s_bl.Task.Delete(id.Value);
                    }
                    catch (BODeletionImpossibleException ex)
                    {
                        throw new BODeletionImpossibleException(ex.Message);
                    }
                    catch (BODoesNotExistException ex)
                    {
                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
                case "engineer":
                    try
                    {
                        s_bl.Engineer.Delete(id.Value);
                    }
                    catch (BODeletionImpossibleException ex)
                    {
                        throw new BODeletionImpossibleException(ex.Message);
                    }
                    catch (BODoesNotExistException ex)
                    {
                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void Update(string entity)
        {
            int? id;
            Console.WriteLine($"insert id of {entity} to update");
            id = int.TryParse(Console.ReadLine(), out int Id1) ? Id1 : null;
            while (id == null)
            {
                Console.WriteLine("must receive ID");
                id = int.TryParse(Console.ReadLine(), out int Id2) ? Id2 : null;
            }
            string format = "dd/hh/mm";
            switch (entity)
            {
                case "task":
                    {
                        try
                        {

                            Task original_t = s_bl.Task.GetTaskDetails(id.Value)!;
                            Console.WriteLine($"insert:\n alias \n description \n planned start date \n start date \n duration \n completed date\n  product \n remarks \n complexity(0-4) \n engineer ID \n tasks I'm depenedes on them (ex: n1, n2, n3...) \n\n in a date data insert the date by the format:{format}");
                            string? alias = Console.ReadLine();
                            string? description = Console.ReadLine();
                            string? plannedStartDate = Console.ReadLine();
                            string? startDate = Console.ReadLine();
                            string? duration = Console.ReadLine();
                            string? completedDate = Console.ReadLine();
                            string? product = Console.ReadLine();
                            string? remarks = Console.ReadLine();
                            int? complexityLevel = int.TryParse(Console.ReadLine(), out int comp) ? comp : null;
                            int? eng_id = int.TryParse(Console.ReadLine(), out int Id2) ? Id2 : null;
                            int[]? dep_id = string.IsNullOrWhiteSpace(Console.ReadLine()) ? null : Console.ReadLine()!.Split(' ').Select(int.Parse).ToArray();
                            string[] d_h_m;
                            TimeSpan? newTS = null;
                            if (duration != "")
                            {
                                d_h_m = duration!.Split(':');
                                newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));
                            }

                            EngineerInTask? eng = null;
                            if (eng_id != null)
                                eng = new EngineerInTask() { Id = eng_id.Value, Name = null };
                            DateTime? readyPlannedStartDate = null;
                            DateTime? readyStartDate = null;
                            DateTime? readyCompletedDate = null;
                            DateTime? p_finish_date = null;
                            List<TaskInList>? alldep = null;
                            if (dep_id != null && dep_id.Length > 0)
                            {
                                alldep = new List<TaskInList>();
                                foreach (var number in dep_id)
                                {
                                    TaskInList dep_task = new TaskInList() { Id = number };
                                    alldep!.Add(dep_task);
                                }
                            }
                            if (plannedStartDate != null && plannedStartDate != "")
                            {
                                if (!IsDate(plannedStartDate)) throw new Exception("wrong date");
                                readyPlannedStartDate = DateTime.ParseExact(plannedStartDate!, format, null);
                                p_finish_date = readyPlannedStartDate.Value.Add(newTS ?? original_t.Duration!.Value);
                            }
                            if (startDate != null && startDate != "")
                            {
                                if (!IsDate(startDate)) throw new Exception("wrong date");
                                readyStartDate = DateTime.ParseExact(startDate!, format, null);
                            }

                            if (completedDate != null && completedDate != "")
                            {
                                if (!IsDate(completedDate)) throw new Exception("wrong date");
                                readyCompletedDate = DateTime.ParseExact(completedDate!, format, null);
                            }

                            Task t = new()
                            {
                                Id = id.Value,
                                Description = (description == "") ? original_t.Description : description,
                                Alias = (alias == "") ? original_t.Alias : alias,
                                Status = original_t.Status,
                                CreatedAtDate = original_t.CreatedAtDate,
                                AllDependencies = alldep ?? original_t.AllDependencies,
                                PlannedStartDate = readyPlannedStartDate ?? original_t.PlannedStartDate,
                                StartDate = readyStartDate ?? original_t.StartDate,
                                PlannedFinishDate = p_finish_date ?? original_t.PlannedFinishDate,
                                Remarks = (remarks == "") ? original_t.Remarks : remarks,
                                Product = (product == "") ? original_t.Product : product,
                                Duration = newTS ?? original_t.Duration,
                                Engineer = eng ?? original_t.Engineer,
                                ComplexityLevel = (EngineerExperience?)complexityLevel ?? original_t.ComplexityLevel,
                            };
                            try
                            {
                                s_bl.Task.Update(t);
                            }
                            catch (BOInvalidUpdateException ex)
                            {

                                throw new BOInvalidUpdateException(ex.Message);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + "invalid input");
                        }
                    }
                    break;
                case "engineer":
                    {
                        Engineer original_e = s_bl.Engineer.GetEngineerDetails(id.Value);
                        Console.WriteLine("insert: \n name \n email \n level(0-4)\n cost \n task to work on \n\n");
                        string? name = Console.ReadLine();
                        string? email = Console.ReadLine();
                        int? level = int.TryParse(Console.ReadLine(), out int res3) ? res3 : null;
                        EngineerExperience? engExp = (BO.EngineerExperience?)level ?? null;
                        double? cost = double.TryParse(Console.ReadLine(), out double c) ? c : null;
                        int? task = int.TryParse(Console.ReadLine(), out int t) ? t : null;
                        TaskInEngineer? t_i_e = null;
                        if (task != null)
                            t_i_e = new TaskInEngineer() { Id = task.Value };
                        Engineer newEng = new()
                        {
                            Id = id.Value,
                            Name = name == "" ? original_e.Name : name,
                            Email = email == "" ? original_e.Email : email,
                            Level = engExp ?? original_e.Level,
                            Cost = cost ?? original_e.Cost,
                            Task = t_i_e ?? original_e.Task,
                        };
                        try { s_bl.Engineer.Update(newEng); }
                        catch (BOAlreadyExistsException ex)
                        { throw new BOAlreadyExistsException(ex.Message); }
                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void Read(string entity)
        {
            int? id;
            Console.WriteLine($"insert id of {entity} to print");
            id = int.TryParse(Console.ReadLine(), out int Id1) ? Id1 : null;
            while (id == null)
            {
                Console.WriteLine("must receive ID");
                id = int.TryParse(Console.ReadLine(), out int Id2) ? Id2 : null;
            }
            switch (entity)
            {
                case "task":
                    try
                    {
                        Console.WriteLine(s_bl!.Task!.GetTaskDetails((int)id));
                    }
                    catch (BODoesNotExistException ex)
                    {
                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
                case "engineer":
                    try
                    {
                        Console.WriteLine(s_bl!.Engineer!.GetEngineerDetails((int)id));
                    }
                    catch (BODoesNotExistException ex)
                    {
                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
            }
            Sub_menu(entity);
        }
        public static void ReadAll(string entity)
        {
            switch (entity)
            {
                case "task":
                    try
                    {
                        foreach (var item in s_bl!.Task!.GetAllTasks())
                            Console.WriteLine(item);
                    }
                    catch (BODoesNotExistException ex)
                    {

                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
                case "engineer":
                    try
                    {
                        foreach (var item in s_bl!.Engineer!.GetAllEngineers())
                            Console.WriteLine(item);
                    }
                    catch (BODoesNotExistException ex)
                    {

                        throw new BODoesNotExistException(ex.Message);
                    }
                    break;
            }
            Sub_menu(entity);
        }
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
