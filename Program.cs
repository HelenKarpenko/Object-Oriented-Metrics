using System;
using System.Reflection;

namespace ObjectOrientedMetrics
{
    class Program
    {
        private static void Main(string[] args)
        {
            var assembly = Assembly.LoadFile(@"C:\Users\lena\source\repos\MockProject\MockProject\bin\Debug\netcoreapp3.1\MockProject.dll");
            var assemblyMetrics = new AssemblyMetrics(assembly);
            assemblyMetrics.PrintMetrics();
            Console.WriteLine();
            foreach (var classInfo in assemblyMetrics.Classes)
            {
                classInfo.PrintMetrics();
            }

            Console.ReadLine();
        }
    }
}
