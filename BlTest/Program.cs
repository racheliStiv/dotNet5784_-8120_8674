using BlApi;
using BO;
using DalApi;
using System.Linq.Expressions;

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
            try
            {
                //main menu input
                Console.WriteLine("choose an entity u wana check \n 1 to Task\n 2 to Engineer \n 3 to start date of groject \n 0 to exit \n");
                bool flag = true;

                while (flag)
                {
                    flag = false;
                    int choose = int.Parse(Console.ReadLine() ?? "");
                    Console.WriteLine();
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
                        case 3:
                            if (s_bl.StartDate != null)
                                Console.WriteLine($"There is already a start date for the project: {s_bl.StartDate.Value.ToString("dd/MM/yy")}");
                            else
                            {
                                try
                                {
                                    Console.WriteLine("insert start date of project");
                                    string? startDate = Console.ReadLine();
                                    if (startDate != null && startDate != "")
                                    {
                                        if (!IsDate(startDate)) throw new Exception("invalid date");
                                        s_bl.StartDate = DateTime.ParseExact(startDate!, "dd/MM/yy", null).Date;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message);
                                }
                            }
                            Main_menu();
                            break;
                        default:
                            Console.WriteLine("wrong input");
                            flag = true;
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main_menu();
            }
        }
        public static void Sub_menu(string entity)
        {
            Console.WriteLine($"choose an act for {entity}, \n 0 to main menu \n 1 to Create\n 2 to Read\n 3 to Read All \n 4  to Update\n 5 to Delete \n");
            bool flag = true;
            while (flag)
            {
                flag = false;
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
                    default:
                        Console.WriteLine("wrong input");
                        flag = true;
                        break;
                }
            }
        }
        public static void Create(string entity)
        {

            switch (entity)
            {
                case "task":
                    try
                    {
                        string format = "hh:mm:ss";
                        Console.WriteLine($"insert:\n alias \n description \n  duration \n  product \n remarks \n complexity(0-4)  \n\n in a date data insert the date by the format: dd/mm/yy \n in duration data by the foramt: {format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        string? duration = Console.ReadLine();
                        string? product = Console.ReadLine();
                        string? remarks = Console.ReadLine();
                        int complexityLevel = int.Parse(Console.ReadLine() ?? "");
                        string[] d_h_m;
                        d_h_m = duration!.Split(':');
                        TimeSpan newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));

                        BO.Task t = new()
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
                    catch (BOCannotAddNewOne ex)
                    {
                        throw new BOCannotAddNewOne(ex.Message);
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
            string format = "dd/MM/yy";
            switch (entity)
            {
                case "task":
                    {
                        try
                        {

                            BO.Task original_t = s_bl.Task.GetTaskDetails(id.Value)!;
                            Console.WriteLine($"in a date data insert the date by the format: {format} \n in duration data by the foramt: hh:mm:ss \n insert: \n alias");
                            string? alias = Console.ReadLine();
                            Console.WriteLine("description");
                            string? description = Console.ReadLine();
                            Console.WriteLine("planned start date");
                            string? plannedStartDate = Console.ReadLine();
                            Console.WriteLine("start date");
                            string? startDate = Console.ReadLine();
                            Console.WriteLine("duration");
                            string? duration = Console.ReadLine();
                            Console.WriteLine("completed date");
                            string? completedDate = Console.ReadLine();
                            Console.WriteLine("product");
                            string? product = Console.ReadLine();
                            Console.WriteLine("remarks");
                            string? remarks = Console.ReadLine();
                            Console.WriteLine("complexity(0-4)");
                            int? complexityLevel = int.TryParse(Console.ReadLine(), out int comp) ? comp : null;
                            Console.WriteLine("engineer ID");
                            int? eng_id = int.TryParse(Console.ReadLine(), out int Id2) ? Id2 : null;
                            Console.WriteLine("tasks I'm depenedes on them (ex: n1, n2, n3...)");
                            string? input = Console.ReadLine();
                            int[]? dep_id = !string.IsNullOrEmpty(input)
                              ? input.Split(',')
                              .Select(str => int.TryParse(str.Trim(), out int num) ? num : (int?)null)
                              .Where(num => num.HasValue)
                              .Select(num => num!.Value)
                              .ToArray()
                              : null;


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
                            DateTime? completed_date = null;
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
                                completed_date = readyPlannedStartDate.Value.Add(newTS ?? original_t.Duration!.Value);
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

                            BO.Task t = new()
                            {
                                Id = id.Value,
                                Description = (description == "") ? original_t.Description : description,
                                Alias = (alias == "") ? original_t.Alias : alias,
                                Status = original_t.Status,
                                CreatedAtDate = original_t.CreatedAtDate,
                                AllDependencies = alldep ?? original_t.AllDependencies,
                                PlannedStartDate = readyPlannedStartDate ?? original_t.PlannedStartDate,
                                PlannedFinishDate = completed_date ?? original_t.PlannedFinishDate,
                                StartDate = readyStartDate ?? original_t.StartDate,
                                CompletedDate = readyCompletedDate ?? original_t.CompletedDate,
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
                            throw new Exception(ex.Message);
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
                            Console.WriteLine(item + "\n");
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
                            Console.WriteLine(item + "\n");
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

            try
            {
                //Reset question
                Console.Write("Would you like to reset your data? (Y/N)");
                string? ans1 = Console.ReadLine();
                while (ans1 != "y" && ans1 != "Y" && ans1 != "n" && ans1 != "N")
                {
                    Console.WriteLine("press y/n");
                    ans1 = Console.ReadLine();
                }

                if (ans1 == "Y" || ans1 == "y")
                    Factory.Get.Reset();


                //Initial question
                Console.Write("Would you like to create Initial data? (Y/N)");
                string? ans2 = Console.ReadLine();
                while (ans2 != "y" && ans2 != "Y" && ans2 != "n" && ans2 != "N")
                {
                    Console.WriteLine("press y/n");
                    ans2 = Console.ReadLine();
                }

                if (ans2 == "Y" || ans2 == "y")
                    DalTest.Initialization.DO();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Main();
            }
            Main_menu();
        }
    }
}
