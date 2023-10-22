using System;

namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            WellCome8120();
            WellCome8674();
            Console.ReadKey();
        }

        private static void WellCome8120()
        {
            Console.WriteLine("Enter Your Name: ");
            string name = System.Console.ReadLine();
            Console.WriteLine("{0}, wellcome to my first console application", name);
        }
        static partial void WellCome8674();
    }
}