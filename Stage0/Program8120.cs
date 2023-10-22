using System;
partial class Program
{
    private static void Main(string[] args)
    {
        Welcome8120();
        Welcome8674();
        Console.ReadKey();
    }

    private static void Welcome8120()
    {
        Console.WriteLine("Enter Your Name: ");
        string name = System.Console.ReadLine();
        Console.WriteLine("{0}, wellcome to my first console application", name);
    }
    static partial void Welcome8674();
}