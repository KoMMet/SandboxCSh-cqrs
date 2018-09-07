namespace ConsoleApp1
{
    public class ChangeAgeCommand : Command
    {
        public Person Target { get; }
        public int Age { get; }

        public ChangeAgeCommand(Person target, int age)
        {
            Target = target;
            Age = age;
        }
    }
}