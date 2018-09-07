namespace ConsoleApp1
{
    public class Person
    {
        public int Age { get; private set; }
        public EventBroker Broker { get; }

        public Person(EventBroker broker)
        {
            this.Broker = broker;
            broker.Commands += BrokerOnCommands;
            broker.Queries += BrokerOnQueries;
        }

        private void BrokerOnQueries(object sender, Query e)
        {
            if (e is AgeQuery ac && ac.Target == this)
            {
                ac.Result = Age;
            }
        }

        private void BrokerOnCommands(object sender, Command command)
        {
            if (command is ChangeAgeCommand cac && cac.Target == this)
            {
                if (cac.Register) Broker.AllEvents.Add(new AgeChangedEvent(this, Age, cac.Age));
                Age = cac.Age;
            }
        }

    }
}