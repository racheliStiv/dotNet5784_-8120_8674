﻿using DalApi;
using DO;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static void Main_menu()
        {
            //main menu input
            Console.WriteLine("choose an entity u wana check \n 1 to Task\n 2 to Engineer \n 0 to exit \n");
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
                        Console.WriteLine($"insert:\n alias \n description \n start date \n scheduled date \n duration \n deadline date \n  complete date \n  product \n remarks \n engineer id \n complexity(0-4)  \n\n in a data date insert the date by the format:{format}");
                        string? alias = Console.ReadLine();
                        string? description = Console.ReadLine();
                        BO.Status status;
                        List<TaskInList> allDep;
                        DateTime createdAtDate = DateTime.Now;
                        string? PlannedStartDate = Console.ReadLine();
                        string? StartDate = Console.ReadLine();
                        string? PlannedFinishDate = Console.ReadLine();
                        string? CompletedDate = Console.ReadLine();
                        string? Product = Console.ReadLine();
                        string? duration = Console.ReadLine();
                        string? Remarks = Console.ReadLine();
                        Engineer eng;
                        int ComplexityLevel = int.Parse(Console.ReadLine() ?? "");
                        string[] d_h_m = duration!.Split(':');
                        TimeSpan newTS = new TimeSpan(int.Parse(d_h_m[0]), int.Parse(d_h_m[1]), int.Parse(d_h_m[2], 0));
                        DO.Task t = new(0, alias, description, DateTime.Now, DateTime.ParseExact(start_date!, format, null), DateTime.ParseExact(scheduled_date!, format, null), newTS, DateTime.ParseExact(deadline_date!, format, null), DateTime.ParseExact(complete_date!, format, null), product, remarks, engineer_id, (EngineerExperience)complexity);
                        s_bl!.Task?.Create(t);
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