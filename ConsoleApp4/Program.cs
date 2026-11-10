using System;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. Імітація обробки (1000 об'єктів по 1 МБ) ===");
            RunTask1();

            Console.WriteLine("\n=== 2. Демонстрація методів GC ===");
            RunTask2();

            Console.WriteLine("\n=== 3. Практичний експеримент (10 000 об'єктів по 100 КБ) ===");
            RunTask3();

            Console.ReadKey();
        }

        static void RunTask1()
        {
            for (int i = 0; i < 1000; i++)
            {
                var processor = new DataProcessor(1024 * 1024);
                processor.Process();
            }

            Console.WriteLine($"Пам'ять: {GC.GetTotalMemory(false)} байт");
        }

        static void RunTask2()
        {
            DataObject testObj = new DataObject(100);
            Console.WriteLine($"Покоління testObj: Gen {GC.GetGeneration(testObj)}");

            Console.WriteLine($"Пам'ять ДО прибирання: {GC.GetTotalMemory(false) / 1024} КБ");
            PrintGCStats("ДО");

           
            for (int i = 0; i < 500; i++)
            {
                var temp = new DataObject(10 * 1024);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Пам'ять ПІСЛЯ прибирання: {GC.GetTotalMemory(false) / 1024} КБ");
            Console.WriteLine($"Покоління testObj після GC: Gen {GC.GetGeneration(testObj)}");
            PrintGCStats("ПІСЛЯ");
        }

        static void RunTask3()
        {
            int g0Start = GC.CollectionCount(0);
            int g1Start = GC.CollectionCount(1);
            int g2Start = GC.CollectionCount(2);

            for (int i = 0; i < 10000; i++)
            {
                var obj = new DataObject(100 * 1024);
            }

            long memBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Використання пам'яті ДО GC: {memBefore / (1024.0 * 1024.0):F2} МБ");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            long memAfter = GC.GetTotalMemory(true);
            Console.WriteLine($"Використання пам'яті ПІСЛЯ GC.Collect(): {memAfter / (1024.0 * 1024.0):F2} МБ");

            Console.WriteLine("Кількість запусків GC за експеримент:");
            Console.WriteLine($"Gen 0: {GC.CollectionCount(0) - g0Start}");
            Console.WriteLine($"Gen 1: {GC.CollectionCount(1) - g1Start}");
            Console.WriteLine($"Gen 2: {GC.CollectionCount(2) - g2Start}");
        }

        static void PrintGCStats(string stage)
        {
            Console.WriteLine($"[{stage}] Gen 0: {GC.CollectionCount(0)}, Gen 1: {GC.CollectionCount(1)}, Gen 2: {GC.CollectionCount(2)}");
        }
    }
}