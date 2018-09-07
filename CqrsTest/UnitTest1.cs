using ConsoleApp1;
using Xunit;
namespace CqrsTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var eventBroker = new EventBroker();
            var person = new Person(eventBroker);
            var changeAgeCommand = new ChangeAgeCommand(person,2);
            Assert.Equal(2, changeAgeCommand.Age);
        }
    }
}
