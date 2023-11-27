using Dal;
using DalApi;
using System.Diagnostics;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        public static void Main_menu()
        {
            Console.WriteLine("choose an entity u wana chack");
            switch ()
            {
                default:
                    break;
            }
        }

        public static void Sub_menu()
        {

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

