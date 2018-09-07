using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var eb = new EventBroker();
            var p = new Person(eb);
            for (int i = 0; i < 300000; i++)
            {
                eb.Command(new ChangeAgeCommand(p, i));
            }

            eb.PrintEventHistries();

            int age;
            age = eb.Query<int>(new AgeQuery {Target = p});
            Console.WriteLine(age);
            eb.UndoLast();

            eb.PrintEventHistries();
            age = eb.Query<int>(new AgeQuery {Target = p});
            Console.WriteLine(age);
            Console.ReadKey();
        }
    }
}