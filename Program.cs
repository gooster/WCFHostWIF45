using System;

namespace WCFHostWIF45
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new MyServiceHost();

            host.Open();

            Console.WriteLine("Press <Enter> to terminate the Host application.");
            Console.WriteLine();
            Console.ReadLine();
            Console.WriteLine("terminating...");

            host.Close();
        }
    }
}
