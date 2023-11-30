using Dal;
using DalApi;
using DO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();




    
        public static void Delete(string entity)
        {
            int id;
            switch (entity)
            {
                case "task":
                    {
                        Console.WriteLine("insert id of task");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dalTask?.Delete(id);
                    }
                    break;
                case "dependency":
                    {
                        Console.WriteLine("insert id of dependency");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dalDependency?.Delete(id);
                    }
                    break;
                case "engineer":
                    {
                        Console.WriteLine("insert id of engineer");
                        id = int.Parse(Console.ReadLine() ?? "");
                        s_dalEngineer?.Delete(id);
                    }
                    break;
            }
        }
        

       


        public static void Main()
        {
            try
            {
                Initialization.DO(s_dalDependency, s_dalTask, s_dalEngineer);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
    }


}

