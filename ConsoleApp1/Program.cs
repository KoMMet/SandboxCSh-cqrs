using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class PersonStrage
    {
        private Dictionary<int, Person> people;
    }
    public class Person
    {
        public int UniquId;
        private int age;
        EventBroker broker;

        public Person(EventBroker broker)
        {
            this.broker = broker;
            broker.Commands += BrokerOnCommands;
            broker.Queries += BrokerOnQueries;
        }

        private void BrokerOnQueries(object sender, Query e)
        {
            var ac = e as AgeQuery;
            if (ac != null && ac.Target == this)
            {
                ac.Result = age;
            }
        }

        private void BrokerOnCommands(object sender, Command command)
        {
            var cac = command as ChangeAgeCommand;
            if (cac != null && cac.Target == this)
            {
                if (cac.Register) broker.AllEvents.Add(new AgeChangedEvent(this, age, cac.Age));
                age = cac.Age;
            }
        }

        public bool CanVote => age >= 18;
    }

    public class EventBroker
    {
        //1. all events that happned
        public IList<Event> AllEvents = new List<Event>();

        //2. commands
        public event EventHandler<Command> Commands;

        //3. query
        public event EventHandler<Query> Queries;

        public void Command(Command c)
        {
            Commands?.Invoke(this, c);
        }

        public T Query<T>(Query q)
        {
            Queries?.Invoke(this, q);
            return (T) q.Result;
        }

        public void UndoLast()
        {
            var e = AllEvents.LastOrDefault();
            var ac = e as AgeChangedEvent;
            if (ac != null)
            {
                Command(new ChangeAgeCommand(ac.Target, ac.OldValue) {Register = false});
                AllEvents.Remove(e);
            }
        }

        public void PrintEventHistries()
        {
            foreach (var e in AllEvents)
            {
                Console.WriteLine(e);
            }
        }
    }

    public class Query
    {
        public object Result;
    }

    class AgeQuery : Query
    {
        public Person Target;
    }

    public class Command : EventArgs
    {
        public bool Register = true;
    }

    class ChangeAgeCommand : Command
    {
        public Person Target;
        public int TargetId;
        public int Age;

        public ChangeAgeCommand(Person target, int age)
        {
            Target = target;
            Age = age;
        }
    }

    public class Event
    {

    }

    class AgeChangedEvent : Event
    {
        public Person Target;
        public int OldValue, NewValue;

        public AgeChangedEvent(Person target, int oldValue, int newValue)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override string ToString()
        {
            return $"age changed from {OldValue} to {NewValue}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var eb = new EventBroker();
            var p = new Person(eb);
            eb.Command(new ChangeAgeCommand(p, 1));
            eb.Command(new ChangeAgeCommand(p, 6));
            eb.Command(new ChangeAgeCommand(p, 12));

            eb.PrintEventHistries();

            int age;
            age = eb.Query<int>(new AgeQuery {Target = p});
            Console.WriteLine(age);
            eb.UndoLast();

            eb.PrintEventHistries();
            age = eb.Query<int>(new AgeQuery { Target = p });
            Console.WriteLine(age);
            Console.ReadKey();
        }
    }
}